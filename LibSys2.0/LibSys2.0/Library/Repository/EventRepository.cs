﻿using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    public class EventRepository : GenericRepository<Event>
    {
        public EventRepository()
        {
            table = "events";
            tableIdName = "event_id";
        }
    }
}
