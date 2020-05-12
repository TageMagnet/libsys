using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Models
{
    public class Category : IArticle
    {
        public int category_id { get; set; }
        public string code { get; set; }
        public string category { get; set; }

    }
}
