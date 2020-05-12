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
        public async Task<List<eBook>> SearchByTitle(string searchString)
        {
            return new List<eBook>();
        }

        public async Task<List<eBook>> ReadAllActiveeBooks()
        {
            using (var connection = CreateConnection())
            {
                List<eBook> ebooks = new List<eBook>();
                List<Author> authors = new List<Author>();

                // From `books`-table get all active that has a reference to an author id
                var query = string.Join(" ", new string[]{
                    "SELECT * FROM `ebooks`",
                    "LEFT JOIN authors ON ebooks.ref_author_id = authors.author_id",
                    "WHERE is_active = 1"
                });

                ebooks = (await connection.QueryAsync<eBook>(query)).ToList();
                authors = (await connection.QueryAsync<Author>(query)).ToList();

                foreach (eBook ebook in ebooks)
                {
                    // Insert the Author object into books
                    Author a = authors.First(x => x.author_id == ebook.ref_author_id);
                    ebook.Author = a;
                }

                connection.Close();
                return ebooks;

            }
        }

        public new async Task Update(eBook ebook)
        {
            using (var connection = CreateConnection())
            {
                string query = string.Join(" ", new string[]{
                    "UPDATE ebooks SET",
                    "`ref_author_id` = @ref_author_id,",
                    "`year` = @year,",
                    "`isbn` = @isbn,",
                    "`title` = @title,",
                    "`description` = @description,",
                    "`url` = @url,",
                    "`content` = @content,",
                    "`category` = @category",
                    "WHERE ebook_id = @ebook_id"
                });

                await connection.QueryAsync(query, ebook);
                connection.Close();
            }
        }
    }
}
