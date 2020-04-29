﻿using LibSys2._0.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    public class BookRepository : GenericRepository<Book>
    {
        public BookRepository()
        {
            table = "books";
            tableIdName = "book_id";
        }
    }
}
