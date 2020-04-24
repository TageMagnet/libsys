using System;
using System.Collections.Generic;
using System.Text;

namespace LIBSYS.Models
{
    public class Member
    {
        public int member_id { get; set; }
        public string email { get; set; }
        public string nickname { get; set; }
        public string pwd { get; set; }
        public List<string> roles { get; set; } = new List<string>();
        public DateTime createdAt { get; set; }
    }
}
