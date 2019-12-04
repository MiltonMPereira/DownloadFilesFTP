using Npgsql;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Download_Anexos
{
    class Bd
    {
        private static NpgsqlConnection conexaoSql_SE = null;
        private static SqlConnection conexaoSql = null;
        public static string usuario_cod = "";
        public static string Ambiente = "";
        public static string url_ftp = "";
        private string string_conexao = "";
        private string string_conexao_SE = "";

        public Bd()
        {
            string argumento = "";
            string[] args = Environment.GetCommandLineArgs();

            foreach (string arg in args)
            {
                if (!(arg.Contains(@"\") || arg.Contains("/")))
                {
                    argumento += arg.ToString();
                }
            }
            Ambiente = argumento.Split(';')[0].ToString();
            usuario_cod = argumento.Split(';')[1].ToString();
        }

        private void GetDados_SE()
        {
            AbreBanco();

            string sql = "SELECT server, db_name, db_user, db_pass, url FROM dadosSistema WHERE cd_sistema = 2 AND ambiente = " + Ambiente;
            SqlDataReader reader = SelectSql(sql);

            if (reader.Read())
            {
                string_conexao_SE = "Server=" + reader["server"].ToString()
                                 + ";Port=port"
                                 + ";Database=" + reader["db_name"].ToString()
                                 + ";User Id=" + reader["db_user"].ToString()
                                 + ";Pooling=false"
                                 + ";Password=" + reader["db_pass"].ToString()
                                 + ";Application Name=Baixa Anexos";
                url_ftp = reader["url"].ToString().Replace("https://", "ftp://").Replace("/se/", "/anexos/");
            }
            else
            {
                string_conexao_SE = "Server=ipProd"
                                 + ";Port=port"
                                 + ";Database=se"
                                 + ";User Id=user"
                                 + ";Pooling=false"
                                 + ";Password=pass"
                                 + ";Application Name=Baixa Anexos";
            }
            FechaConexao();
        }

        public void AbreBanco_SE()
        {
            GetDados_SE();
            conexaoSql_SE = new NpgsqlConnection();
            conexaoSql_SE.ConnectionString = this.string_conexao_SE;
            conexaoSql_SE.Open();
        }

        public void AbreBanco()
        {
            if (Ambiente == "0")
            {
                string_conexao = "Data Source=ipProd;Initial Catalog=base;MultipleActiveResultSets=True;Connection Timeout=0;user=se;Pooling=false;password=se;Application Name=Baixa Anexos";
            }
            else
            {
                string_conexao = "Data Source=ipTest;Initial Catalog=base;MultipleActiveResultSets=True;Connection Timeout=0;user=se;Pooling=false;password=se;Application Name=Baixa Anexos";
            }
            conexaoSql = new SqlConnection();
            conexaoSql.ConnectionString = this.string_conexao;
            conexaoSql.Open();
        }

        public NpgsqlDataReader SelectSql_SE(string sql)
        {
            NpgsqlCommand comando = new NpgsqlCommand();
            comando.CommandText = sql;
            comando.Connection = conexaoSql_SE;
            NpgsqlDataReader reader = null;
            reader = comando.ExecuteReader();
            return reader;
        }

        public SqlDataReader SelectSql(string sql)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandText = sql;
            comando.Connection = conexaoSql;
            SqlDataReader reader = null;
            reader = comando.ExecuteReader();
            return reader;
        }

        public void FechaConexao()
        {
            if (conexaoSql != null && conexaoSql.State != ConnectionState.Closed)
                conexaoSql.Close();
        }

        public void FechaConexao_SE()
        {
            if (conexaoSql_SE != null && conexaoSql_SE.State != ConnectionState.Closed)
                conexaoSql_SE.Close();
        }
    }
}
