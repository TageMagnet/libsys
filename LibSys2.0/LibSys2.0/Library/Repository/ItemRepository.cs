using Dapper;
using Library;
using LibrarySystem;
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
        public async Task ChangeStatusItem(int id, int status)
        {
            using (var connection = CreateConnection())
            {
                string sqlQuery = $"UPDATE {table} SET is_active={status} WHERE {tableIdName} = " + id.ToString();
                await connection.QueryAsync(sqlQuery);
            }
        }

        /// <summary>Already logged in member, subscribe/loan to item</summary>
        /// <param name="item">Selected item</param>
        /// <param name="member">Logged in member</param>
        public async Task SubscribeToItem(BaseItem item, Member member)
        {
            using (var connection = CreateConnection())
            {
                string query = string.Join(" ", new string[]
                {
                    "INSERT INTO item_subscriptions(`ref_member_id`, `ref_book_id`, `loaned_at`, `return_at`, `status`)",
                    "VALUES(@ref_member_id, @ref_book_id, @loaned_at, @return_at, @status);"
                });

                // Add time duration until item is to be returned
                // todo; move this logic elsewhere. Repos' should be as simple as possible
                DateTime returnDate = DateTime.Now.Add(Globals.DefaultLoanDuration);
                await connection.QueryAsync(query, new { ref_member_id = member.member_id, ref_book_id = item.ID, loaned_at = DateTime.Now, return_at = returnDate, status = 1 });
            }
        }

        /// <summary>
        /// Extend loan period for item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public async Task ResubscribeToItem(OverViewItem item, Member member)
        {
            using (var connection = CreateConnection())
            {
                string query = @"
                UPDATE
                	item_subscriptions A
                SET
                	`status` = @status,
                    `return_at` = @ReturnAt
                WHERE
                	A.ref_member_id = @memberID
                AND
                	A.ref_book_id = @bookID

                AND
                	A.ID = @SubscriptionID;
                ";
                await connection.QueryAsync(query, new { memberID = member.member_id, bookID = item.ref_book_id, ReturnAt = item.return_at, SubscriptionID = item.SubscriptionID, status = 1 });
            }
        }

        /// <summary>Return/unsubscribe to item</summary>
        /// <param name="item">Selected item</param>
        /// <param name="member">Logged in member</param>
        public async Task UnSubscribeToItem(OverViewItem item, Member member)
        {
            using (var connection = CreateConnection())
            {
                string query = @"UPDATE
                	item_subscriptions A
                SET
                	`status` = @status
                WHERE
                	A.ref_member_id = @memberID
                AND
                	A.ref_book_id = @bookID
                AND
                	A.ID = @SubscriptionID
                ";
                await connection.QueryAsync(query, new { memberID = member.member_id, bookID = item.ref_book_id, SubscriptionID = item.SubscriptionID, status = 0 });
            }
        }

        public async Task<List<Item>> ReadAllItemsWithStatus(int status) => await ReadAllItemsWithStatus(status, 999999);

        public async Task<List<Item>> ReadAllItemsWithStatus(int status, int limiter)
        {
            using (var connection = CreateConnection())
            {
                List<Item> items = new List<Item>();
                List<Author> authors = new List<Author>();

                // From `books`-table get all active that has a reference to an author id
                var query = string.Join(" ", new string[]{
                    "SELECT * FROM `items`",
                    "LEFT JOIN authors ON items.ref_author_id = authors.author_id",
                    "WHERE is_active = @status",
                    "LIMIT @limiter;"
                });

                items = (await connection.QueryAsync<Item>(query, new { status = status, limiter = limiter })).ToList();
                authors = (await connection.QueryAsync<Author>(query, new { status = status, limiter = limiter })).ToList();

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

        /// <summary>
        /// .................
        /// ................................................
        /// ...........................................
        /// ....................................help me
        /// </summary>
        /// <param name="status"></param>
        /// <param name="limiter"></param>
        /// <returns></returns>
        public async Task<List<OverViewItem>> ReadAllItemsWithStatus2(int status, int limiter)
        {
            using (var connection = CreateConnection())
            {
                List<OverViewItem> items = new List<OverViewItem>();
                List<Author> authors = new List<Author>();

                // From `books`-table get all active that has a reference to an author id
                var query = string.Join(" ", new string[]{
                    "SELECT * FROM `items`",
                    "LEFT JOIN authors ON items.ref_author_id = authors.author_id",
                    "WHERE is_active = @status",
                    "LIMIT @limiter;"
                });

                items = (await connection.QueryAsync<OverViewItem>(query, new { status = status, limiter = limiter })).ToList();
                authors = (await connection.QueryAsync<Author>(query, new { status = status, limiter = limiter })).ToList();

                foreach (OverViewItem item in items)
                {
                    // Insert the Author object into items
                    Author a = authors.First(x => x.author_id == item.ref_author_id);
                    item.Author = a;
                }

                connection.Close();
                return items;

            }
        }

        public async Task DeleteReason(Item item)
        {
            using (var connection = CreateConnection())
            {
                string query = string.Join(" ", new string[]{
                    "UPDATE items SET",
                  "`reasonToDelete`= @reasonToDelete",
                    "WHERE ID = @ID"
                });

                await connection.QueryAsync(query, item);
                connection.Close();
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
        /// Searches several columns in the selected table
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

                // Retrieves books/items
                items = (await connection.QueryAsync<Item>(query, new { Q = searchString })).ToList();

                // Run the query again but collect authors this time
                // Items.Author property is an Author object, but havent figured out how to make Dapper fill this automatically
                authors = (await connection.QueryAsync<Author>(query, new { Q = searchString })).ToList();

                // Joining Items with Author object
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

        /// <summary>
        /// Searches several columns in the selected table
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public async Task<List<SearchItem>> SearchQueryWithStatuses(string searchString)
        {
            List<Item> items = new List<Item>();
            List<SearchItem> searchItems = new List<SearchItem>();
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

                // Load items matching search query
                items = (await connection.QueryAsync<Item>(query, new { Q = searchString })).ToList();

                // Run the query again but collect authors this time
                // Items.Author property is an Author object, but havent figured out how to make Dapper fill this automatically
                authors = (await connection.QueryAsync<Author>(query, new { Q = searchString })).ToList();

                // Query that maybe keep tracks of duplicates
                string testQuery = @"
                SELECT
                    items.isbn as isbn,
                    COUNT(isbn) as Total,
                	(SELECT COUNT(*) FROM item_subscriptions WHERE item_subscriptions.ref_book_id = items.ID AND item_subscriptions.status > 0) as UnAvailable
                FROM
                    items
                GROUP BY
                    isbn";

                // res.Item1 = total count
                // res.Item2 = subscribed count
                // var res = connection.Query<object, object, object, Tuple<object, object, object>>(testQuery, Tuple.Create, splitOn: "*").ToList();
                var xx = (await connection.QueryAsync<dynamic>(testQuery)).ToList();

                // Joining Items with Author object
                foreach (Item item in items)
                {
                    // Insert the Author object into items
                    Author a = authors.First(x => x.author_id == item.ref_author_id);
                    item.Author = a;
                    searchItems.Add(new SearchItem(item));
                }

                // Loop the already subscribed items and match with items
                // todo; error prone 
                foreach (var r in xx)
                {

                    var found = searchItems.Find(item => item.isbn == r.isbn);

                    // Error check
                    if (found == null)
                        continue;

                    // Amount of duplicates
                    found.Total = (int)r.Total;
                    // If loaned out/subscribed to
                    found.UnAvailable = (int)r.UnAvailable;
                    // Availability display
                    found.Available = (int)r.Total - found.UnAvailable;

                    // Extra thing fix for cloned book, unavailable was not displaying correct
                    // If more than 1, it's a books with duplicates
                    if(r.Total > 1)
                    {
                        string countSQLquery = @"
                            SELECT
                            	*
                            FROM
                            	item_subscriptions
                            JOIN items ON
                            	items.ID = item_subscriptions.ref_book_id
                            WHERE
                            	items.isbn = @isbn
                            AND
                                status > 0;";

                        var itemSubscriptions = (await connection.QueryAsync(countSQLquery, new { isbn = found.isbn })).ToList();

                        found.UnAvailable = (int)itemSubscriptions.Count();
                        //found.Available = found.Available - (int)found.UnAvailable;
                    }

                }

                connection.Close();
                return searchItems;
            }
        }

        public async Task<List<OverViewItem>> ReadSubscribedItems(int memberID) => await ReadSubscribedItems(memberID, 999999);
        public async Task<List<OverViewItem>> ReadSubscribedItems(int memberID, int limiter)
        {
            // LIMIT markerar max antal böcker laddade
            string query = @"
            SELECT
            	`members`.*,
            	`items`.*,
            	`item_subscriptions`.*,
            	item_subscriptions.ID as SubscriptionID
            FROM
            	item_subscriptions
            LEFT JOIN items ON
            	items.ID = item_subscriptions.ref_book_id
            LEFT JOIN members ON
            	members.member_id = item_subscriptions.ref_member_id
            WHERE
            	members.member_id = 1
            	AND item_subscriptions.status = 1
            LIMIT 
                @limit;
            ";

            using (var connection = CreateConnection())
            {
                List<OverViewItem> data = (await connection.QueryAsync<OverViewItem>(query, new { memberID = memberID, status = 1, @limit = limiter })).ToList();
                return data;
            }

        }
    }
}
