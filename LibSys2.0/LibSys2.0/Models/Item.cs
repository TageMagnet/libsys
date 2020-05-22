using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Models
{
    public class Item
    {
        public string type { get; set; }
        public int ID { get; set; }
        public int ref_author_id { get; set; }
        public int is_active { get; set; } = 1;
        public int year { get; set; }
        public short book_state { get; set; } = 100;
        public string isbn { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string url { get; set; } = "/";
        public string content { get; set; } = "Default Content";
        public string category { get; set; }
        public string cover { get; set; } = null;
        public Author Author { get; set; } = null;
        public string reasonToDelete { get; set; }

    }
}
