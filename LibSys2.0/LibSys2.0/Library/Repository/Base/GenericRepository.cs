﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;


namespace Library
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        //private IDbConnection connection { get; }
        //private MySqlConnection client;

        /// <summary>
        /// Name of SQL table
        /// </summary>
        protected string table;

        /// <summary>
        /// Name of id property in SQL-table
        /// </summary>
        protected string tableIdName;

        /// <summary>
        /// MySQL
        /// </summary>
        /// <returns></returns>
        protected MySqlConnection CreateConnection()
        {
            string connectionString = "server=tjackobacco.com;port=23006;database=libsys;uid=guest;pwd=hunter12;";
            MySqlConnection con = new MySqlConnection(connectionString);
            con.Open();
            return con;
        }

        /// <summary>
        /// Microsoft SQL Server
        /// </summary>
        /// <param name="s">add a string to use MSSQL instead</param>
        /// <returns></returns>
        protected IDbConnection CreateConnection(string s)
        {
            string connectionString = "Data Source=syss3-grupp1.database.windows.net;Initial Catalog=libsys;User Id=Grupp1;Password=Hunter12;";
            IDbConnection con = new SqlConnection(connectionString);
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
                    await connection.QueryAsync<int>(query, t);
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
                // Probably risk for SQL-injection, but mweh..
                string sqlQuery = $"DELETE FROM {table} WHERE {tableIdName} = " + id.ToString();
                await connection.QueryAsync(sqlQuery);
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
        public async Task Update(T t)
        {
            using (var connection = CreateConnection())
            {
                try
                {
                    string query = GenerateUpdateQuery(t);
                    await connection.QueryAsync(query, t);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Trace.WriteLine(e.Message);
                    throw;
                }
            }
        }

        /// <summary>
        /// Return a list of all rows
        /// </summary>
        /// <returns></returns>
        public async Task<List<T>> ReadAll()
        {
            using (var connection = CreateConnection())
            {
                return (await connection.QueryAsync<T>($"SELECT * FROM {table}")).ToList();
            }
        }

        /// <summary>
        /// Create generic SQL-query for INSERT based on <see cref="T"> properties
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public string GenerateInsertQuery(T t)
        {
            var insertQuery = new StringBuilder($"INSERT INTO {table} ");

            insertQuery.Append("(");

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
                //insertQuery.Append($"[{prop}],");
                insertQuery.Append($"{prop},");
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public string GenerateUpdateQuery(T t)
        {
            var sqlQuery = new StringBuilder($"UPDATE {table} SET ");
            List<string> properties = new List<string>();
            var tableId = t.GetType().GetProperty(tableIdName).GetValue(t, null);

            // Loop all the properties in supplied class
            foreach (var prop in t.GetType().GetProperties())
            {
                // Is null check
                bool isNull = prop.GetValue(t, null) != null ? false : true;
                // Is is or reference id check
                bool isID = prop.Name.Contains("_id") || prop.Name.Contains("ref_") ? true : false;

                // property value is not null nor name contains '_id' 
                if (!isNull && !isID)
                {
                    properties.Add(prop.Name);
                }
            }
            properties.ForEach(prop =>
            {
                sqlQuery.Append($"`{prop}` = @{prop},");
            });

            sqlQuery
                .Remove(sqlQuery.Length - 1, 1)
                .Append($" WHERE {tableIdName} = {tableId.ToString()}");

            return sqlQuery.ToString();
        }
    }
}

