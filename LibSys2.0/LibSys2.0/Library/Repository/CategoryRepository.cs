
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

        public async Task<Category> GetCategory(string category)
        {
            using (var connection = CreateConnection())
            {
                var result = await connection.QuerySingleAsync<Category>("SELECT FROM categories WHERE code = @category", new { category = category });
                return result;
            }
        }
    }
}
