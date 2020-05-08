using Dapper;
using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class eBookRepository : GenericRepository<eBook>
    {
        public eBookRepository()
        {
            table = "ebooks";
            tableIdName = "ebook_id";
        }
        /// <summary>
        /// Changes is_active property from 1 to 0.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task ChangeStatuseBook(int id)
        {
            using (var connection = CreateConnection())
            {
                string sqlQuery = $"UPDATE {table} SET is_active=0 WHERE {tableIdName} = " + id.ToString();
                await connection.QueryAsync(sqlQuery);
            }
        }
        public async Task<List<eBook>> ReadAllActiveeBooks()
        {
            using (var connection = CreateConnection())
            {
                return (await connection.QueryAsync<eBook>($"SELECT * FROM {table} WHERE is_active=1")).ToList();
            }
        }
        public async Task<List<eBook>> SearchByTitle(string searchString)
        {
            return new List<eBook>();
        }
    }
}
