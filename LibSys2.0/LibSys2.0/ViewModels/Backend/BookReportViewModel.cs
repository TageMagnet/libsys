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
        public List<OverViewItem> Items { get; set; } = new List<OverViewItem>();
        public ItemRepository itemRepo = new ItemRepository();
        public List<Item> ListOfInactiveBooks { get; set; } = new List<Item>();
        public RelayCommand PrintReportCommand { get; set; }

        #endregion

        public BookReportViewModel()
        {
            PrintReportCommand = new RelayCommand(async () => PrintReportMethod());
            ReadItems();
            ReadOtherData();
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

        public async void ReadOtherData()
        {
            await GetOtherData();
        }
        public async Task PrintReportMethod()
        {
            MessageBox.Show("Skriver ut rapport.");
        }

        public async Task GetOtherData()
        {
            foreach (var item in await itemRepo.ReadAllItemsWithStatus2(1, 25))
            {
                // todo; incorrect. Used as a placeholder for now
                item.loaned_at = Etc.Utilities.RandomDate();
                Items.Add(item);
            }
        }
    }
}
