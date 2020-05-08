using System;
using System.Collections.Generic;
using System.Data;
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
    }
}
