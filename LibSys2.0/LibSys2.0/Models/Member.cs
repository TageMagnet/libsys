using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Models
{
    public class Member
    {
        public int member_id { get; set; }
        public string email { get; set; }
        public string nickname { get; set; }
        public string pwd { get; set; }
        /// <summary>
        /// Temporary string for insert into SQL
        /// </summary>
        public string role { get; set; } = "guest";

        public int SelectedIndex { get; set; }
        public DateTime created_at { get; set; }
        public Int16 is_active { get; set; }
    }
}
