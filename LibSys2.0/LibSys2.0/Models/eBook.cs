using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Models
{
    public class eBook : IArticle
    {
        public int ebook_id { get; set; }
        public int ref_author_id { get; set; }
        public int year { get; set; }
        public string isbn { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string content { get; set; }
        public string category { get; set; }
        public string cover { get; set; }
    }
}
