using Library;
using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.ViewModels.Backend
{
    class BookReportViewModel : BaseViewModel
    {

        #region Properties
        public ItemRepository itemRepo = new ItemRepository();
        public List<Item> ListOfInactiveBooks { get; set; } = new List<Item>();
        

        #endregion

        public BookReportViewModel()
        {
            ReadItems();
        }

        public async void ReadItems()
        {
            await GetItems();
        }
        public async Task GetItems()
        {
           foreach(var item in await itemRepo.ReadAll())
            {
                if(item.is_active == 0)
                {
                    ListOfInactiveBooks.Add(item);
                }
            }
        }
    }
}
