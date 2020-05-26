using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Models
{
    public class OverViewItem : BaseItem
    {
        /// <summary>
        /// Used for visual display
        /// </summary>
        public string LateStatus { get; set; } = "Ja";
        /// <summary>
        /// Days remaining on subscription
        /// </summary>
        public int SubscriptionDaysRemaining { get; set; }
        public string isbn { get; set; }
        public string title { get; set; }

        public DateTime loaned_at { get; set; }
        public DateTime return_at { get; set; }
        public DateTime updated_at { get; set; }

        public Author Author { get; set; } = null;
    }
}
