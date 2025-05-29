using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLySachTruyen
{
    internal class DAO
    {
        public static SqlConnection con = new SqlConnection();
        public static string ConnectionString =
                                        "Data Source = DESKTOP-9C5NLIH\\SQLEXPRESS01;" +
                                        "Initial Catalog=quanlysachtruyen;" +
                                        " Integrated Security = True;" +
                                        "Encrypt=False";

        public static void Connect()
        {
            con.ConnectionString = ConnectionString;
            try
            {
                if (con != null & con.State == ConnectionState.Closed)
                    con.Open();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Close()
        {
            try
            {
                if (con != null & con.State == ConnectionState.Open)
                    con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable LoadDataToTable(string sql)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(sql, DAO.con);
            adapter.Fill(dt);
            return dt;
        }
    }
}
