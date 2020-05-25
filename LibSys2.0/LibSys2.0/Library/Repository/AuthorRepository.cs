using Dapper;
using Library;
using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class AuthorRepository : GenericRepository<Author>
    {
        public AuthorRepository()
        {
            table = "authors";
            tableIdName = "author_id";
        }

        /// <summary>
        /// Read authors that matchess searchString
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public async Task<List<Author>> Search(string searchString)
        {
            using (var connection = CreateConnection())
            {
                searchString += '%';
                string query = @"
                SELECT
                	*
                FROM
                	authors A
                WHERE (A.firstname LIKE @Q)
                OR (A.surname LIKE @Q)
                OR (A.nickname LIKE @Q);";

                List<Author> authors = (await connection.QueryAsync<Author>(query, new { Q = searchString })).ToList();

                connection.Close();
                return authors;
            }
        }
    }
}
