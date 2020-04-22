using System;
using System.Collections.Generic;
using System.Text;

namespace LIBSYS.Models
{
    class User
    {
        public eStatus Status { get; set; }
        public enum eStatus { guest = 0, admin = 1, librarian = 2, customer = 3 }
        public string Email { get; set; }
        public string NickName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
