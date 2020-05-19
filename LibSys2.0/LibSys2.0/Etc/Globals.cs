using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace LibrarySystem
{
    /// <summary>
    /// Doing it hard and statically like it's no one's business
    /// </summary>
    public static class Globals
    {
        public static Member LoggedInUser {get;set;} = new Member();
    }
}


