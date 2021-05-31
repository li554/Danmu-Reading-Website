using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Mind.Models
{
    public class Database
    {
        private const string conf = "Data Source = .; Initial Catalog = book_db; Integrated Security = SSPI";
        private readonly SqlConnection sqlConnection = new SqlConnection(conf);
        public SqlDataReader Fetch(string sql)
        {
            try
            {
                var cmd = new SqlCommand(sql, sqlConnection);
                var sqlData = cmd.ExecuteReader();
                return sqlData;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public void Open()
        {
            sqlConnection.Open();
        }

        public void Close()
        {
            sqlConnection.Close();
        }

        public int Update(string sql)
        {
            try
            {
                var cmd = new SqlCommand(sql, sqlConnection);
                var code = cmd.ExecuteNonQuery();
                sqlConnection.Close();
                return code;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                sqlConnection.Close();
                return -1;
            }
        }
    }
}