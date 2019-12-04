using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Forms;

namespace Download_Anexos
{
    class Arquivo
    {
        Funcoes f = new Funcoes();

        public string GetIDs()
        {
            try
            {
                List<string> oids = new List<string>();
                Bd bd = new Bd();
                bd.AbreBanco();
                SqlDataReader reader = bd.SelectSql("SELECT valor FROM dadosArquivo WHERE user = " + Bd.usuario_cod);

                while (reader.Read())
                {
                    oids.Add("'" + reader["valor"].ToString() + "'");
                }
                bd.FechaConexao();
                return string.Join(",", oids);
            }
            catch (Exception e)
            {
                f.MsgAviso("Erro ao consultar ID no banco de dados.");
                return "";
            }
        }

        public void BuscaArquivos()
        {
            string fluxosConsulta = GetIDs();
            List<string> caminhoArquivosFluxo = new List<string>();
            Ftp ftp = new Ftp();

            if (fluxosConsulta != "")
            {
                try
                {
                    Bd bd = new Bd();
                    bd.AbreBanco_SE();

                    NpgsqlDataReader reader = bd.SelectSql_SE("SELECT arquivo FROM tabelaAnexos WHERE id IN (" + fluxosConsulta + ")");

                    while (reader.Read()) { caminhoArquivosFluxo.Add(reader["arquivo"].ToString()); }

                    bd.FechaConexao_SE();
                    
                    ftp.BaixaArquivosFTP(caminhoArquivosFluxo);
                }
                catch (Exception e)
                {
                    f.MsgAviso("Erro na busca de arquivos.");
                }
            }
        }

        public bool DescompactaArquivos(string diretorio)
        {
            try
            {
                if (diretorio != "")
                {
                    List<FileInfo> zips = new List<FileInfo>();
                    string[] files = Directory.GetFiles(Path.GetDirectoryName(diretorio));
                    string filePos;

                    if (files.Length > 0)
                    {
                        f.MsgStatus("Descompactando arquivos...");

                        for (int i = 0; i < files.Length; i++)
                        {
                            filePos = files[i];
                            zips.Add(new FileInfo(filePos));
                            ZipFile.ExtractToDirectory(filePos, diretorio); //extrai

                            f.prog_status.PerformStep();
                            Application.DoEvents();
                        }
                        zips.ForEach(x => x.Delete()); //deleta zips que ficaram
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                f.MsgStatus("");
                f.MsgAviso("Erro ao descompactar arquivos.");
                return false;
            }
        }

        public int RenomeiaArquivos(string diretorio)
        {
            try
            {
                NpgsqlDataReader reader;
                string tempFileName;
                string restanteNome;

                if (diretorio != "")
                {
                    DirectoryInfo d = new DirectoryInfo(diretorio);
                    List<string> arquivosRenomeados = new List<string>();
                    List<FileInfo> arquivos = new List<FileInfo>();

                    arquivos = d.GetFiles().ToList();

                    if (arquivos.Count > 0)
                    {
                        f.MsgStatus("Renomeando arquivos...");
                        Bd bd = new Bd();
                        bd.AbreBanco_SE();

                        for (int i = 0; i < arquivos.Count; i++)
                        {
                            reader = bd.SelectSql_SE("select id from tabelaAnexos where id = '" + arquivos[i].Name.Split('_')[0] + "'");

                            if (reader.Read())
                            {
                                restanteNome = string.Join("_", arquivos[i].Name.Split('_').Skip(1));
                                tempFileName = reader["id"].ToString() + " - " + restanteNome.Replace("-", "").Replace(reader["id"].ToString(), "").TrimStart();
                                tempFileName = Path.GetFileNameWithoutExtension(tempFileName).TrimEnd() + Path.GetExtension(tempFileName);
                                arquivosRenomeados.Add(tempFileName);
                            }
                            reader.Close();
                        }
                        bd.FechaConexao_SE();

                        arquivosRenomeados = new List<string>(VerificaDuplicados(arquivosRenomeados));

                        for (int i = 0; i < arquivosRenomeados.Count; i++)
                        {
                            arquivos[i].MoveTo(diretorio + arquivosRenomeados[i]);

                            f.prog_status.PerformStep();
                            Application.DoEvents();
                        }
                    }
                    return arquivos.Count;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception e)
            {
                f.MsgStatus("");
                f.MsgAviso("Erro ao renomear arquivos.");
                return 0;
            }
        }

        private List<string> VerificaDuplicados(List<string> arquivosRenomeados)
        {
            List<string> result = arquivosRenomeados.Take(1).ToList();

            for (var i = 1; i < arquivosRenomeados.Count; i++)
            {
                var name = arquivosRenomeados[i];
                var count = arquivosRenomeados.Take(i).Where(n => n.ToUpper() == name.ToUpper()).Count() + 1;

                result.Add(count < 2 ? name : Path.GetFileNameWithoutExtension(name) + " (" + count.ToString() + ")" + Path.GetExtension(name));
            }
            return result;
        }
    }
}
