using System;
using System.Collections.Generic;
using System.Text;

namespace LIBSYS.Models
{
    class Member
    {
        public int member_ID { get; set; }
        public string email { get; set; }
        public string nickName { get; set; }
        public string pwd { get; set; }
        public string role { get; set; }
        public DateTime createdAt { get; set; }
    }
}
