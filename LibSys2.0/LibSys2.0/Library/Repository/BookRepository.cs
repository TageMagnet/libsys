using Dapper;
using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class BookRepository : GenericRepository<Book>
    {
        public BookRepository()
        {
            table = "books";
            tableIdName = "book_id";
        }

        /// <summary>
        /// Return a list of rows matching the searchString
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public async Task<List<Book>> SearchByTitle(string searchString)
        {
            List<Book> books = new List<Book>();
            using (var connection = CreateConnection())
            {
                //Add %-wildcard operator to the end
                searchString += '%';
                return (await connection.QueryAsync<Book>("SELECT * FROM books WHERE title LIKE @title", new { title = searchString })).ToList();
            }
        }
    }
}
