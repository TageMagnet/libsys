using System;
using System.Collections.Generic;
using System.Text;

namespace LIBSYS.Models
{
    class User
    {
        public eStatus Status { get; set; }
        public enum eStatus { Admin = 1, Librarian = 2, Customer = 3, Guest = 4 }
        public string Email { get; set; }
        public string NickName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
