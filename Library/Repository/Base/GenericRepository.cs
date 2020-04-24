using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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

        public async Task Create(T t)
        {
            using (var connection = Connection)
            {
                await connection.QuerySingleAsync<T>($"INSERT INTO {table} VALUES (@value);", new { value = t });
            }
        }

        public async Task Delete(int id)
        {
            using (var connection = Connection)
            {
                await connection.QuerySingleAsync<T>($"DELETE FROM {table} WHERE {table + "_id"} = @id;", new { id = id });
            }
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

        public async Task<List<T>> ReadAll()
        {
            using (var connection = Connection)
            {
                var res = await connection.QueryAsync<T>($"SELECT * FROM {table}");
                return res.ToList();
            }
        }
    }
}

