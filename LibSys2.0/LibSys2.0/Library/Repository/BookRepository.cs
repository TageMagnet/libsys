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
        /// Changes status on property is_active from 1 to 0
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task ChangeStatusBook(int id)
        {
            using (var connection = CreateConnection())
            {
                string sqlQuery = $"UPDATE {table} SET is_active=0 WHERE {tableIdName} = " + id.ToString();
                await connection.QueryAsync(sqlQuery);
            }
        }




        public async Task<List<Book>> ReadAllActiveBooks()
        {
            using (var connection = CreateConnection())
            {
                List<Book> books = new List<Book>();

                // Get all books from repo
                books = (await connection.QueryAsync<Book>($"SELECT * FROM {table} WHERE is_active=1")).ToList();

                // Check all books and match ref_author_id against authors.id
                foreach (Book book in books)
                {
                    if (book.ref_author_id == 0)
                    {
                        continue;
                    }
                    Author author = await connection.QuerySingleAsync<Author>("SELECT * FROM `books` RIGHT JOIN authors ON books.ref_author_id = authors.author_id  WHERE is_active=1 AND book_id = @bookId", new { bookId = book.book_id });
                    book.Author = author;
                }


                connection.Close();
                return books;

            }
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
