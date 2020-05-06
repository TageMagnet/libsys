using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Models
{
    /// <summary>
    /// Writer of books
    /// </summary>
    public class Author
    {
        public int author_id { get; set; }
        public string firstname { get; set; }
        public string surname { get; set; }
    }
}
