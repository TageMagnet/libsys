using System;
using System.Collections.Generic;
using System.Text;

namespace LIBSYS.Models
{
    public class Repository
    {
        public Repository()
        {
            ConnectionString = "Data Source=SQL6009.site4now.net;Initial Catalog=DB_A53DDD_Grupp1;User Id=DB_A53DDD_Grupp1_admin;Password=Password123;";
            connection = new SqlConnection(ConnectionString);
            connection.Open();
        }
    }
}
