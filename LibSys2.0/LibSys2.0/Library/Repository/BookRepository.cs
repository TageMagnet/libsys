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
                List<Author> authors = new List<Author>();

                // From `books`-table get all active that has a reference to an author id
                var query = string.Join(" ", new string[]{
                    "SELECT * FROM `books`",
                    "LEFT JOIN authors ON books.ref_author_id = authors.author_id",
                    "WHERE is_active = 1"
                });

                books = (await connection.QueryAsync<Book>(query)).ToList();
                authors = (await connection.QueryAsync<Author>(query)).ToList();

                foreach (Book book in books)
                {
                    // Insert the Author object into books
                    Author a = authors.First(x => x.author_id == book.ref_author_id);
                    book.Author = a;
                }

                connection.Close();
                return books;

            }
        }

        public new async Task Update(Book book)
        {
            using (var connection = CreateConnection())
            {
                string query = string.Join(" ", new string[]{
                    "UPDATE books SET",
                    "`ref_author_id` = @ref_author_id,",
                    "`year` = @year,",
                    "`isbn` = @isbn,",
                    "`title` = @title,",
                    "`description` = @description,",
                    "`url` = @url,",
                    "`content` = @content,",
                    "`book_state` = @book_state,",
                    "`category` = @category",
                    "WHERE book_id = @book_id"
                });

                await connection.QueryAsync(query, book);
                connection.Close();
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

        /// <summary>
        /// ...
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public async Task<List<Book>> SearchQuery(string searchString)
        {
            List<Book> books = new List<Book>();
            List<Author> authors = new List<Author>();
            using (var connection = CreateConnection())
            {
                //Add %-wildcard operator to the end
                searchString += '%';
                string query = string.Join(" ", new string[] {
                    "SELECT * FROM books JOIN authors A ON ref_author_id = A.author_id",
                    "WHERE title LIKE @Q",
                    "OR A.firstname LIKE @Q",
                    "OR A.surname LIKE @Q",
                    "OR A.nickname LIKE @Q"
                });

                //
                books = (await connection.QueryAsync<Book>(query, new { Q = searchString })).ToList();

                // run the query again but collect authors this time
                authors = (await connection.QueryAsync<Author>(query, new { Q = searchString })).ToList();

                foreach (Book book in books)
                {
                    // Insert the Author object into books
                    Author a = authors.First(x => x.author_id == book.ref_author_id);
                    book.Author = a;
                }

                connection.Close();
                return books;
            }
        }
    }
}
