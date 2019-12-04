using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace Download_Anexos
{
    class Ftp
    {
        Funcoes f = new Funcoes();
        public static bool parar = false;

        public void BaixaArquivosFTP(List<string> arquivos)
        {
            f.prog_status.Maximum = arquivos.Count * 3;

            try
            {
                Arquivo a = new Arquivo();
                string url = Bd.url_ftp;
                DialogResult dr = DialogResult.None;

                if (url != "")
                {
                    string filename;
                    string dirAnexos = "";
                    int contNaoExistentes = 0;
                    bool pastaSelecionada = false;
                    bool erro = false;

                    Form tela = (Form)Application.OpenForms.OfType<DownloadAnexos>().First();
                    tela.Show();
                    f.FocusAplicacao();
                    f.MsgStatus("Buscando arquivos do servidor...");

                    for (int i = 0; i < arquivos.Count; i++)
                    {
                        FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(new Uri(url + arquivos[i]));
                        ftp.Credentials = new NetworkCredential("user", "pass");
                        ftp.UseBinary = true;
                        ftp.Method = WebRequestMethods.Ftp.DownloadFile;
                        ftp.Proxy = null;
                        ftp.KeepAlive = false;
                        try
                        {
                            using (WebResponse response = ftp.GetResponse())
                            {
                                if (pastaSelecionada == false)
                                {
                                    tela.Hide();
                                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                                    dr = fbd.ShowDialog();
                                    f.FocusAplicacao();
                                    if (dr == DialogResult.OK)
                                    {
                                        dirAnexos = fbd.SelectedPath + @"\Anexos_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + @"\";
                                        Directory.CreateDirectory(dirAnexos);
                                        pastaSelecionada = true;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                filename = Path.GetFileName(response.ResponseUri.ToString());

                                using (FileStream output = File.OpenWrite(dirAnexos + filename))
                                {
                                    using (Stream input = response.GetResponseStream())
                                    {
                                        input.CopyTo(output);
                                    }
                                }
                            }
                            tela.Show();
                            f.FocusAplicacao();
                            f.MsgStatus("Baixando Arquivos...");
                        }
                        catch (WebException e)
                        {
                            FtpWebResponse response = (FtpWebResponse)e.Response;
                            if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                            {
                                contNaoExistentes++;
                            }
                            else
                            {
                                f.MsgStatus("");
                                f.MsgAviso("Erro ao baixar arquivos.");
                                erro = true;
                                break;
                            }
                        }
                        f.prog_status.PerformStep();
                        Application.DoEvents();

                        if (parar == true) break;
                    }
                    if (a.DescompactaArquivos(dirAnexos))
                    {
                        int qtdArquivosBaixados = a.RenomeiaArquivos(dirAnexos);
                        if (parar != true && dr != DialogResult.Cancel)
                        {
                            if (qtdArquivosBaixados > 0)
                            {
                                f.MsgStatus("Arquivos baixados com sucesso.");

                                if (contNaoExistentes == 0)
                                {
                                    f.MsgInformacao("Arquivos salvos no diretório: " + Path.GetFileName(Path.GetDirectoryName(dirAnexos)));
                                }
                                else
                                {
                                    f.prog_status.Value += f.prog_status.Maximum - f.prog_status.Value;
                                    f.MsgInformacao("Arquivos não encontrados: " + contNaoExistentes + Environment.NewLine
                                                  + "Arquivos baixados: " + qtdArquivosBaixados + Environment.NewLine
                                                  + "Diretório: " + Path.GetFileName(Path.GetDirectoryName(dirAnexos)));
                                }
                            }
                            else
                            {
                                f.prog_status.Value += (f.prog_status.Maximum - f.prog_status.Value);
                                if (erro == false) f.MsgAviso("Não há arquivos no servidor para os fluxos selecionados.");
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                f.MsgStatus("");
                f.MsgAviso("Erro ao baixar arquivos.");
            }
        }
    }
}
