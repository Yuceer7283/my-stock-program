using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST
{
    internal class ConnectionString
    {
        public SqlConnection GetCon() 
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=.;Initial Catalog=TeknikServis;Persist Security Info=True;User ID=sa;Password=123456";
            return con;

            // "Data Source=DESKTOP-N1ST5EV\SQLEXPRESS;Initial Catalog=StokTakip;Integrated Security=True;TrustServerCertificate=true"
        }
    }
}
