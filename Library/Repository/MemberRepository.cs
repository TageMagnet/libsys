using System;
using System.Collections.Generic;
using System.Text;
using LIBSYS.Models;

namespace Library
{
    public class MemberRepository : GenericRepository<Member>
    {
        public MemberRepository()
        {
            table = "members";
            tableIdName = "member_id";
        }
    }
}
