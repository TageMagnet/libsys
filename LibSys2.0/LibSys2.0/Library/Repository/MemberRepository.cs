using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using LibrarySystem.Models;

namespace Library
{
    public class MemberRepository : GenericRepository<Member>
    {
        public MemberRepository()
        {
            table = "members";
            tableIdName = "member_id";
        }

        public async Task<List<Member>> ReadAllActive()
        #region ..
        {
            using (var connection = CreateConnection())
            {
                return (await connection.QueryAsync<Member>($"SELECT * FROM {table} WHERE is_active = 1")).ToList();
            }
        }
        #endregion
    }
}
