using Library;
using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LibrarySystem.ViewModels.Backend
{
    class BookReportViewModel : BaseViewModel
    {

        #region Properties
        public ItemRepository itemRepo = new ItemRepository();
        public List<Item> ListOfInactiveBooks { get; set; } = new List<Item>();
        public RelayCommand PrintReportCommand { get; set; }

        #endregion

        public BookReportViewModel()
        {
            PrintReportCommand = new RelayCommand(async () => PrintReportMethod());
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
        public async Task PrintReportMethod()
        {
            MessageBox.Show("Skriver ut rapport.");
        }
    }
}
