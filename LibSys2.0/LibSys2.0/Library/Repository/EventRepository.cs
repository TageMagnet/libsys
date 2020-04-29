﻿using LibSys2._0.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    public class EventRepository : GenericRepository<Event>
    {
        public EventRepository()
        {
            table = "event";
            tableIdName = "event_id";
        }
    }
}
