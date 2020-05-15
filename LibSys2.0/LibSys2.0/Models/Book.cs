using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Models
{
    public class Book : IArticle
    {
        public int book_id { get; set; }
        public int ref_author_id { get; set; }
        public int year { get; set; } = 0;
        public string isbn { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string url { get; set; } = "/";
        public string content { get; set; } = "Deafault Content";

        /// <summary>
        /// Current condition of physical; book from 0 to 255
        /// </summary>
        public short book_state { get; set; } = 100;

        public Author Author { get; set; }

        public string category { get; set; }
        public string cover { get; set; } = null;

        public int is_active { get; set; } = 1;
    }
}
