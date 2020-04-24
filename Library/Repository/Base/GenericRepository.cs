using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Library
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private string ConnectionString { get; set; }
        private IDbConnection connection { get; }

        /// <summary>
        /// Name of SQL table
        /// </summary>
        protected string table;

        /// <summary>
        /// Name of id property in SQL-table
        /// </summary>
        protected string tableIdName;


        protected IDbConnection CreateConnection()
        {
            ConnectionString = "Data Source=syss3-grupp1.database.windows.net;Initial Catalog=libsys;User Id=Grupp1;Password=Hunter12;";
            IDbConnection con = new SqlConnection(ConnectionString);
            con.Open();
            return con;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task Create(T t)
        {
            using (var connection = CreateConnection())
            {
                try
                {
                    string query = GenerateInsertQuery(t);
                    var res = (await connection.QueryAsync<int>(query, t)).Single();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Trace.WriteLine(e.Message);
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Delete(int id)
        {
            using (var connection = CreateConnection())
            {
                await connection.QuerySingleAsync<T>($"DELETE FROM {table} WHERE {table + "_id"} = @id;", new { id = id });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> Read(int id)
        {
            using (var connection = CreateConnection())
            {
                try
                {
                    var res = await connection.QuerySingleAsync<T>($"SELECT * FROM {table} WHERE {tableIdName} = @id;", new { id = id });
                    return res;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Trace.WriteLine(e.Message);
                    return null;
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Task Update(T t)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<T>> ReadAll()
        {
            using (var connection = CreateConnection())
            {
                return (await connection.QueryAsync<T>($"SELECT * FROM {table}")).ToList();
            }
        }

        public string GenerateInsertQuery(T t)
        {
            var insertQuery = new StringBuilder($"INSERT INTO {table} ");

            insertQuery.Append("(");

            //var properties = GenerateListOfProperties(GetProperties)
            List<string> properties = new List<string>();
            foreach (var prop in t.GetType().GetProperties())
            {
                if (prop.GetValue(t, null) != null && prop.Name.Contains("_id") == false)
                {
                    properties.Add(prop.Name);
                }
            }
            properties.ForEach(prop =>
            {
                insertQuery.Append($"[{prop}],");
            });

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(") VALUES (");

            properties.ForEach(prop => { insertQuery.Append($"@{prop},"); });

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(")");
            return insertQuery.ToString();
        }
    }
}

