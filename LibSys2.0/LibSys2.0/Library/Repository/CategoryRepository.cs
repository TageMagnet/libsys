
using Dapper;
using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class CategoryRepository : GenericRepository<Category>
    {
        public CategoryRepository()
        {
            table = "categories";
            tableIdName = "category_id";
        }

        public async Task<Category> GetCategory(string cat)
        {
            using (var connection = CreateConnection())
            {
                return await connection.QuerySingleAsync<Category>($"SELECT * FROM {table} WHERE code = @category", new { category = cat });
            }
        }
    }
}
