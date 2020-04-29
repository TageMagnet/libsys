using System;
using System.Collections.Generic;
using System.Text;

namespace LibSys2._0.Models
{
    public class Event
    {
        public int event_id { get; set; }
        public string event_type { get; set; }
        public DateTime time_start { get; set; }
        public DateTime time_end { get; set; }
        public string location { get; set; }
        public int owner { get; set; }
    }
}
