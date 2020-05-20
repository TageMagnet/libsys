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
                    var query = string.Join(" ",
                    "SELECT member_id,",
                        "email,",
                        "nickname,",
                        "pwd,",
                        "role_name as 'role',",
                        "created_at,",
                        "is_active,",
                        "ref_member_role_id",
                    "FROM members M INNER JOIN member_roles MR ",
                    "ON M.ref_member_role_id = MR.member_role_id ",
                    "WHERE M.is_active = 1");
                        
                    return (await connection.QueryAsync<Member>(query)).ToList();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        #endregion

        /// <summary>
        /// Create , but also returns member
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public new async Task<Member> Create(Member member)
        {
            using (var connection = CreateConnection())
            {
                try
                {
                    string query = GenerateInsertQuery(member);
                    await connection.ExecuteAsync(query, member);
                    return (await connection.QueryAsync<Member>("SELECT * FROM members WHERE members.email = @email", new { email = member.email })).FirstOrDefault();
                   
                }
                catch (Exception e)
                {
                    System.Diagnostics.Trace.WriteLine(e.Message);
                    return member;
                }
            }
        }

        public new async Task Update(Member member)
        {
            using (var connection = CreateConnection())
            {
                string query = string.Join(" ", new string[]{
                    "UPDATE members SET",
                    "`email` = @email,",
                    "`nickname` = @nickname,",
                    "`pwd` = @pwd,",
                    "`role` = @role,",
                    "`ref_member_role_id` = @ref_member_role_id,",
                    "`is_active` = @is_active",
                    "WHERE member_id = @member_id"
                });

                await connection.QueryAsync(query, member);
                connection.Close();
            }
        }
    }
}
