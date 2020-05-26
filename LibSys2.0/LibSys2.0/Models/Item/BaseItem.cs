using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Models
{
    public abstract class BaseItem
    {
        public string type { get; set; }
        public int ID { get; set; }
        public int ref_author_id { get; set; }
        public int is_active { get; set; } = 1;
        public int year { get; set; }
    }
}
