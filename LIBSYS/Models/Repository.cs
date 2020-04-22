using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace LIBSYS.Models
{
    public class Repository
    {
        private string ConnectionString { get; }
        private IDbConnection connection { get; }
        public Repository()
        {
            ConnectionString = "Data Source=syss3-grupp1.database.windows.net;Initial Catalog=libsys;User Id=Grupp1;Password=Hunter12;";
            connection = new SqlConnection(ConnectionString);
            connection.Open();
        }


    }
}
