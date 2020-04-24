using LIBSYS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    public class eBookRepository : GenericRepository<eBook>
    {
        public eBookRepository() => table = "ebooks";
    }
}
