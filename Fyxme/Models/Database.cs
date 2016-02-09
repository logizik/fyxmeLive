using System;
using System.Data.SqlClient;
using System.Configuration;

namespace Fyxme.Models
{
    public class Database
    {
        private SqlConnection conn;

        public Database()
        {
            string connString = ConfigurationManager.ConnectionStrings["defaultConnString"].ConnectionString;
            conn = new SqlConnection(connString);

            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Can not open connection ! ");
            }

        }

        public SqlDataReader GetData(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            return reader;
        }

        public void Execute(string sql, params object[] args)
        {
            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Clear();
            for (int i = 1; i <= args.Length; i++)
            {
                cmd.Parameters.AddWithValue("@" + i, args[i - 1]);
            }

            cmd.ExecuteNonQuery();
        }

        public object ExecuteScalar(string sql, params object[] args)
        {
            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Clear();
            for (int i = 1; i <= args.Length; i++)
            {
                cmd.Parameters.AddWithValue("@" + i, args[i - 1]);
            }

            return cmd.ExecuteScalar();
        }

        public void Close()
        {
            conn.Close();
        }
    }
}