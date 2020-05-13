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
                try
                {
                    List<Member> member = (await connection.QueryAsync<Member>($"SELECT member_id, email, nickname, pwd, role_name as 'role', created_at, is_active, ref_member_role_id FROM members M INNER JOIN Member_roles MR ON M.ref_member_role_id = MR.member_role_id WHERE M.is_active = 1")).ToList();
                    return member;
                }
                catch (Exception e)
                {

                    throw;
                }
            }
        }
        #endregion
    }
}
