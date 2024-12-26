using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using DTO;
using System.Security.Principal;



namespace DAL
{
    public class SqlConnectionData
    {
        public static SqlConnection Connect()
        {


            //string strcon = @"Data Source=HUNG;Initial Catalog=airplanedb;Integrated Security=True";    
            //string strcon = @"Data Source=LAPTOP-978A4PM7;Initial Catalog=airplanedb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
            //string strcon = @"Data Source=SPIDEY;Initial Catalog=airplanedb;Integrated Security=True";
            //string strcon = @"Data Source=ZALAW;Initial Catalog=airplanedb;Integrated Security=True";
            string strcon = @"Data Source=172.28.62.1;Initial Catalog=airplanedb;User ID=SA;Password=@Kimyen2004;";
            //string strcon = @"Data Source=PHUONGVY\SQLEXPRESS;Initial Catalog=airplanedb;Integrated Security=True";
            //string strcon = @"Data Source=172.28.62.1;Initial Catalog=airplanedb;User ID=SA;Password=@Kimyen2004;";
            //string strcon = @"Data Source=LAPTOP-3J19JUTN\TAMNHU;Initial Catalog=airplanedb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
            SqlConnection conn = new SqlConnection(strcon); // khởi tạo connect
            return conn;
        }
    }
    public class DatabaseAccess
    {
        // Method to open a connection
        private static SqlConnection OpenConnection()
        {
            SqlConnection conn = SqlConnectionData.Connect();
            conn.Open();
            return conn;
        }
    }
}
