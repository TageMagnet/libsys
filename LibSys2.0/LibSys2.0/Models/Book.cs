using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Models
{
    public class Book
    {
        public int book_id { get; set; }
        public int ref_author_id { get; set; }
        public int year { get; set; }
        public string isbn { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string url { get; set; } = "/";
        public string content { get; set; } = "Deafault Content";

        /// <summary>
        /// Current condition of physical; book from 0 to 255
        /// </summary>
        public int book_state { get; set; } = 100;

        public string category { get; set; }
    }
}
