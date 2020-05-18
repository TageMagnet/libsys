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
    public class ItemRepository : GenericRepository<Item>
    {
        public ItemRepository()
        {
            table = "items";
            tableIdName = "ID";
        }
        /// <summary>
        /// Changes status on property is_active from 1 to 0
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task ChangeStatusItem(int id)
        {
            using (var connection = CreateConnection())
            {
                string sqlQuery = $"UPDATE {table} SET is_active=0 WHERE {tableIdName} = " + id.ToString();
                await connection.QueryAsync(sqlQuery);
            }
        }

        public async Task<List<Item>> ReadAllItemsWithStatus(int status)
        {
            using (var connection = CreateConnection())
            {
                List<Item> items = new List<Item>();
                List<Author> authors = new List<Author>();

                // From `books`-table get all active that has a reference to an author id
                var query = string.Join(" ", new string[]{
                    "SELECT * FROM `items`",
                    "LEFT JOIN authors ON items.ref_author_id = authors.author_id",
                    "WHERE is_active = @status"
                });

                items = (await connection.QueryAsync<Item>(query, new { status = status })).ToList();
                authors = (await connection.QueryAsync<Author>(query, new { status = status })).ToList();

                foreach (Item item in items)
                {
                    // Insert the Author object into items
                    Author a = authors.First(x => x.author_id == item.ref_author_id);
                    item.Author = a;
                }

                connection.Close();
                return items;

            }
        }

        public new async Task Update(Item item)
        {
            using (var connection = CreateConnection())
            {
                string query = string.Join(" ", new string[]{
                    "UPDATE items SET",
                    "`type` = @type,",
                    "`ref_author_id` = @ref_author_id,",
                    "`year` = @year,",
                    "`isbn` = @isbn,",
                    "`title` = @title,",
                    "`description` = @description,",
                    "`url` = @url,",
                    "`cover` = @cover,",
                    "`content` = @content,",
                    "`book_state` = @book_state,",
                    "`category` = @category",
                    "WHERE ID = @ID"
                });

                await connection.QueryAsync(query, item);
                connection.Close();
            }
        }

        /// <summary>
        /// Return a list of rows matching the searchString
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public async Task<List<Item>> SearchByTitle(string searchString)
        {
            List<Item> items = new List<Item>();
            using (var connection = CreateConnection())
            {
                //Add %-wildcard operator to the end
                searchString += '%';
                return (await connection.QueryAsync<Item>("SELECT * FROM items WHERE title LIKE @title", new { title = searchString })).ToList();
            }
        }

        /// <summary>
        /// ...
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public async Task<List<Item>> SearchQuery(string searchString)
        {
            List<Item> items = new List<Item>();
            List<Author> authors = new List<Author>();
            using (var connection = CreateConnection())
            {
                //Add %-wildcard operator to the end
                searchString += '%';
                string query = string.Join(" ", new string[] {
                    "SELECT * FROM items B JOIN authors A ON ref_author_id = A.author_id",
                    "WHERE (B.title LIKE @Q AND B.is_active = 1)",
                    "OR (A.firstname LIKE @Q AND B.is_active = 1)",
                    "OR (A.surname LIKE @Q AND B.is_active = 1)",
                    "OR (A.nickname LIKE @Q AND B.is_active = 1)"
                });

                //
                items = (await connection.QueryAsync<Item>(query, new { Q = searchString })).ToList();

                // run the query again but collect authors this time
                authors = (await connection.QueryAsync<Author>(query, new { Q = searchString })).ToList();

                foreach (Item item in items)
                {
                    // Insert the Author object into items
                    Author a = authors.First(x => x.author_id == item.ref_author_id);
                    item.Author = a;
                }

                connection.Close();
                return items;
            }
        }
    }
}
