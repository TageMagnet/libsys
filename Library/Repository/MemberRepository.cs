using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Library
{
    public class MemberRepository : GenericRepository<Member>
    {
        public MemberRepository()
        {
            table = "members";
            tableIdName = "member_id";
        }

        public new async Task<int> Create(Member member)
        {
            using (var connection = CreateConnection())
            {
                try
                {
                    var memberID = await connection.QuerySingleAsync<int>("create_member", member, commandType: CommandType.StoredProcedure);
                    return memberID;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Trace.WriteLine(e.Message);
                    return 0;
                }

            }
        }
    }
}
