using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace Library
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private string ConnectionString { get; }
        private IDbConnection connection { get; }

        protected string table;

        private IDbConnection Connection
        {
            get
            {
                IDbConnection con = new SqlConnection(ConnectionString);
                con.Open();
                return con;
            }
        }

        public GenericRepository()
        {
            ConnectionString = "Data Source=syss3-grupp1.database.windows.net;Initial Catalog=libsys;User Id=Grupp1;Password=Hunter12;";
            connection = new SqlConnection(ConnectionString);
            connection.Open();
        }

        public Task Create(T t)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<T> Read(int id)
        {
            using (var connection = Connection)
            {
                var res = await connection.QuerySingleAsync<T>($"SELECT * FROM {table} WHERE {table + "_id"} = @id;", new { id= id });
                return res;
            }
        }

        public Task Update(T t)
        {
            throw new NotImplementedException();
        }
    }
}

