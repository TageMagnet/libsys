using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Library
{
    /// <summary>
    /// For stuff withoout a specified class/model
    /// </summary>
    public class MiscellaneousRepository : GenericRepository<object>
    {
        /// <summary>
        /// ...
        /// </summary>
        /// <param name="linkStr"></param>
        /// <param name="userId"></param>
        /// <param name="expireDate"></param>
        /// <returns></returns>
        public async Task<string> CreateRegisterLink(string linkStr, int userId, DateTime expireDate)
        {
            string link = "";
            using (var connection = CreateConnection())
            {
                string query = string.Join(" ", new string[]
                {
                    "START TRANSACTION;",
                    "INSERT INTO registrations(link, ref_member_id, expiresAt)",
                    "VALUES(@link, @userID, @expiresAt);",
                    "SELECT link FROM registrations R WHERE R.link = @link;",
                    "COMMIT;"
                });

                var r = await connection.QuerySingleAsync<dynamic>(query, new { link = linkStr, userID = userId, expiresAt = expireDate });
                link = r.link;
            }
            return link;
        }

        //public new async Task Create(object obj) => throw new NotImplementedException();
        //public new async Task Read() => throw new NotImplementedException();
        //public new async Task Update(object obj) => throw new NotImplementedException();
        //public new async Task Delete(int id) => throw new NotImplementedException();
    }
}
