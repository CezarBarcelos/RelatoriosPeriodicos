using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RelatoriosPeriodicos.Servicos
{
    public class Banco
    {
        public string stringConexao = string.Empty;
        public bool conectado = false;
        public string conexaoErro = string.Empty;
        SqlConnection conexao;        

        public Banco(string strCon)
        {
            stringConexao = strCon;
            Conectar();            
        }

        public bool Conectar()
        {
            try
            {                
                conexao = new SqlConnection(stringConexao);
                conexao.Open();
                conectado = true;                
                return true;
            }
            catch(Exception ex)
            {
                conexaoErro = "StringConnection: " + stringConexao + "\n\nDetalhes: " + ex.Message + "\n\n";
                conectado = false;                
                return false;
            }
        }

        public void Desconectar()
        {
            if(conexao != null && conexao.State != System.Data.ConnectionState.Closed)
            {
                conexao.Close();
            }
        }

        public int INSERT(string sql)
        {
            using (SqlCommand command = new SqlCommand(sql, conexao))
            {
                return command.ExecuteNonQuery();                
            }
        }

        public int UPDATE(string sql)
        {
            using (SqlCommand command = new SqlCommand(sql, conexao))
            {
                return command.ExecuteNonQuery();             
            }
        }

        public int DELETE(string sql)
        {
            using (SqlCommand command = new SqlCommand(sql, conexao))
            {                
                return command.ExecuteNonQuery();             
            }
        }

        public void COMMIT()
        {
            using (SqlCommand command = new SqlCommand("COMMIT TRANSACTION;", conexao))
            {
                command.ExecuteNonQuery();
            }
        }

        public void ROLLBACK()
        {
            using (SqlCommand command = new SqlCommand("ROLLBACK TRANSACTION;", conexao))
            {
                command.ExecuteNonQuery();
            }
        }

        public void BEGIN_TRANSACTION()
        {
            using (SqlCommand command = new SqlCommand("BEGIN TRANSACTION;", conexao))
            {
                command.ExecuteNonQuery();
            }
        }

        public DataTable SELECT(string sql)
        {
            using (SqlCommand command = new SqlCommand(sql, conexao))
            {
                command.CommandTimeout = 30;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    DataTable tbEsquema = reader.GetSchemaTable();
                    DataTable tbRetorno = new DataTable();

                    foreach (DataRow r in tbEsquema.Rows)
                    {
                        if (!tbRetorno.Columns.Contains(r["ColumnName"].ToString()))
                        {
                            DataColumn col = new DataColumn()
                            {
                                ColumnName = r["ColumnName"].ToString(),
                                Unique = Convert.ToBoolean(r["IsUnique"]),
                                AllowDBNull = Convert.ToBoolean(r["AllowDBNull"]),
                                ReadOnly = Convert.ToBoolean(r["IsReadOnly"])
                            };
                            tbRetorno.Columns.Add(col);
                        }
                    }

                    while (reader.Read())
                    {
                        DataRow novaLinha = tbRetorno.NewRow();
                        for (int i = 0; i < tbRetorno.Columns.Count; i++)
                        {
                            novaLinha[i] = reader.GetValue(i);
                        }
                        tbRetorno.Rows.Add(novaLinha);
                    }

                    return tbRetorno;
                }
            }
        }
    }
}
