using System;
using System.Collections.Generic;
using System.Text;

namespace LIBSYS.ViewModels
{
    public class LoginViewModel
    {
        public eStatus Status { get; set; }
        public enum eStatus { Admin = 1, Librarian = 2, Customer = 3, Guest = 4 }
        public int Role { get; set; }

    }



}
