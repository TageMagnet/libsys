using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class eBookRepository : GenericRepository<eBook>
    {
        public eBookRepository()
        {
            table = "ebooks";
            tableIdName = "ebook_id";
        }

        public async Task<List<eBook>> SearchByTitle(string searchString)
        {
            return new List<eBook>();
        }
    }
}
