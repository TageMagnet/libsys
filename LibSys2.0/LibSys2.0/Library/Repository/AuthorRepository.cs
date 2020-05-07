using Library;
using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    public class AuthorRepository : GenericRepository<Author>
    {
        public AuthorRepository()
        {
            table = "authors";
            tableIdName = "author_id";
        }
    }
}
