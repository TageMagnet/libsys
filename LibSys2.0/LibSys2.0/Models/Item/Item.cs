using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Models
{
    public class Item : BaseItem
    {
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

        public Item() { }

        public Item(SearchItem searchItem)
        {
            this.type = searchItem.type;
            this.ID = searchItem.ID;
            this.ref_author_id = searchItem.ref_author_id;
            this.is_active = searchItem.is_active;
            this.year = searchItem.year;
            this.book_state = searchItem.book_state;
            this.isbn = searchItem.isbn;
            this.title = searchItem.title;
            this.description = searchItem.description;
            this.url = searchItem.url;
            this.content = searchItem.content;
            this.category = searchItem.category;
            this.cover = searchItem.cover;
            this.Author = searchItem.Author;
        }
    }
}
