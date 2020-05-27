using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Models
{
    /// <summary>
    /// Just like <see cref="Item"></see>, but with a few added propertes => 
    /// <see cref="Total"></see> is one
    /// </summary>
    public class SearchItem : BaseItem
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
        /// <summary>
        /// Used for tracking duplicates in display view, duplicates should be checked from ISBN code
        /// Defaultar till 1, eftersom det alltid finns minst 1 bok
        /// </summary>
        public int Total { get; set; } = 1;
        /// <summary>
        /// Available for member to subscribe to
        /// </summary>
        public int Available { get; set; } = 1;
        /// <summary>
        /// Unavailable, already loaned out, missing or removed from stock
        /// </summary>
        public int UnAvailable { get; set; }
        /// <summary>
        /// Index to keep track of in ViewFiltering
        /// </summary>
        public int Index { get; set; }
        public SearchItem() { }

        public SearchItem(Item item)
        {
            this.type = item.type;
            this.ID = item.ID;
            this.ref_author_id = item.ref_author_id; 
            this.is_active = item.is_active;
            this.year = item.year;
            this.book_state = item.book_state;
            this.isbn = item.isbn;
            this.title = item.title;
            this.description = item.description;
            this.url = item.url;
            this.content = item.content;
            this.category = item.category;
            this.cover = item.cover;
            this.Author = item.Author;
        }
    }
}
