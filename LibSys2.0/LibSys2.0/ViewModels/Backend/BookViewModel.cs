﻿using Library;
using System;
using System.Collections.Generic;
using System.Text;
using LibrarySystem;
using LibrarySystem.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MessageBox = System.Windows.MessageBox;
using LibrarySystem.ViewModels;
using System.Text.RegularExpressions;
using System.Linq;
using System.Windows.Controls;
using LibrarySystem.Views.Backend;
using LibrarySystem.ViewModels.Backend;
using System.Windows.Input;
using System.Windows;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using System.Threading;

namespace LibrarySystem
{
    public class BookViewModel : BaseViewModel
    {
        private ItemRepository itemRepo = new ItemRepository();
        private AuthorRepository authorRepo = new AuthorRepository();
        private CategoryRepository categoryRepo = new CategoryRepository();
        public Author SelectedAuthor { get; set; } = new Author();
        public Item SelectedItem { get; set; } = new Item();
        public Category BookCategory { get; set; } = new Category();
        public string visible { get; set; } = "hidden";

        public string InputCategory { get; set; }
        public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();
        public ObservableCollection<Author> Authors { get; set; } = new ObservableCollection<Author>();

        public int SelectedAuthorIndex { get; set; } = -1;

        private bool activeBookFilter = false;
        public bool ActiveBookFilter
        {
            get { return activeBookFilter; }
            set
            {
                activeBookFilter = value;
                if (value == true)
                {
                    ActiveFilter = 0;
                    visible = "visible";
                }
                else
                {
                    ActiveFilter = 1;
                    visible = "hidden";
                }
                LoadBooks();
            }
        }
        // Private holder
        private bool limitBookFilter = false;
        /// <summary>
        /// True = only load 5 rows on VM startup, False = Load all books,
        /// </summary>
        public bool LimitBookFilter
        {
            get
            {
                return limitBookFilter;
            }
            set
            {
                limitBookFilter = value;

                // Reload books
                ReloadBooksAsync();
            }
        }

        private string addBookAuthorSearch;
        /// <summary>
        /// Filtering aout available authors in combobox
        /// </summary>
        public string AddBookAuthorSearch
        {
            get
            {
                return addBookAuthorSearch;
            }
            set
            {
                // When
                addBookAuthorSearch = value;
                ReloadAuthorsAsync();
            }
        }

        public int GetTotalAuthorCount => Authors.Count();
        public int GetTotalBookCount => Items.Count();

        /// <summary>Small textbox for posting multiple new items</summary>
        public int NumberOfItemsToSubmit { get; set; } = 1;
        public int ActiveFilter { get; set; } = 1;

        public bool SelectedItemIsEBook { get; set; } = false;

        public RelayCommand AddBookCommand { get; set; }
        public RelayCommandWithParameters UpdateBookCommand { get; set; } // item
        public RelayCommandWithParameters RemoveBookCommand { get; set; }
        public RelayCommandWithParameters ActivateBookCommand { get; set; }
        public RelayCommandWithParameters ToggleHidden { get; set; }
        public RelayCommandWithParameters ToggleVisible { get; set; }
        public RelayCommandWithParameters FileUploadCommand { get; set; }
        public RelayCommandWithParameters UpdateFileCommand { get; set; }
        public RelayCommandWithParameters UpdateUrlCommand { get; set; }
        public RelayCommand BookReportCommand { get; set; }
        public RelayCommand GoToReportPageCommand { get; set; }

        public BookViewModel()
        {
            AddBookCommand = new RelayCommand(async () => await AddBookCommandMethod());
            UpdateBookCommand = new RelayCommandWithParameters(async (param) => await UpdateBookCommandMethod((Item)param));
            RemoveBookCommand = new RelayCommandWithParameters(async (param) => await RemoveBookCommandMethod((Item)param));
            ActivateBookCommand = new RelayCommandWithParameters(async (param) => await ActivateBook((Item)param));
            ToggleHidden = new RelayCommandWithParameters(async (param) => await HiddenCommandMethod((Button)param));
            ToggleVisible = new RelayCommandWithParameters(async (param) => await VisibleCommandMethod((Button)param));
            FileUploadCommand = new RelayCommandWithParameters(async (param) => await UploadFile((string)param));
            UpdateFileCommand = new RelayCommandWithParameters(async (param) => await UpdateFile((Item)param));
            UpdateUrlCommand = new RelayCommandWithParameters(async (param) => await UpdateUrl((Item)param));
            BookReportCommand = new RelayCommand(async () => await BookReportMethod());
            GoToReportPageCommand = new RelayCommand(() => GoToReportPage());
            LoadDataAsync();

        }
        /// <summary>Makes Arrow down button Visible</summary>
        /// <param name="arg"></param>
        private async Task VisibleCommandMethod(Button arg)
        {
            arg.IsEnabled = true;
            SelectedItem.reasonToDelete = "";
            this.OnPropertyChanged(nameof(SelectedItem.reasonToDelete));
        }

        /// <summary>Makes arrow down button Hidden</summary>
        /// <param name="arg"></param>
        private async Task HiddenCommandMethod(Button arg)
        {
            arg.IsEnabled = false;
        }

        /// <summary>
        /// Checks and adds a Book to DB
        /// </summary>
        /// <returns></returns>
        public async Task AddBookCommandMethod()
        {
            #region Felcheck
            if (SelectedItem.title == null)
            {
                MessageBox.Show("Lägg till titel!");
                return;
            }
            if (SelectedAuthor.author_id == 0)
            {
                MessageBox.Show("Lägg till författare!");
                return;
            }
            if (SelectedItem.description == null)
            {
                MessageBox.Show("Lägg till beskrivning!");
                return;
            }
            if (SelectedItem.isbn == null)
            {
                MessageBox.Show("Lägg till isbn!");
                return;
            }
            if (InputCategory == null)
            {
                MessageBox.Show("Lägg till kategori!");
                return;
            }
            if (SelectedItem.year == 0)
            {
                MessageBox.Show("Lägg till årtal!");
                return;
            }
            #endregion
            // We need a real ISBN input string since we are going to do stuff with it later on, e.g. filterering and checking duplicates
            ISBN isbn = new ISBN();

            // Check if ISBN is valid
            if (!isbn.IsValid(SelectedItem.isbn))
            {
                if (SelectedItem.isbn.Length != 13)
                    MessageBox.Show("Fel längd på ISBN string. Endast 13 nummer (eller 12 utan kontroll siffra i slutet)");

                MessageBox.Show("Felaktig ISBN, felaktig kontrollsumma");
                return;
            }
            if (SelectedItemIsEBook == true)
            {
                SelectedItem.type = "ebook";
            }
            else
            {
                SelectedItem.type = "book";
            }
            // Retrieve the full ISBN
            SelectedItem.isbn = SelectedItem.isbn;

            SelectedItem.ref_author_id = SelectedAuthor.author_id;
            SelectedItem.is_active = 1;
            await GetBookCategory(InputCategory);
            if (SelectedItem.category == null)
            {
                return;
            }

            // Loop multiple insert
            for (int i = 0; i < NumberOfItemsToSubmit; i++)
            {
                await itemRepo.Create(SelectedItem);
            }

            // reset
            NumberOfItemsToSubmit = 1;

            await LoadBooks();
            await ClearBookLines("books");
        }

        public async Task ActivateBook(Item arg)
        {
            await itemRepo.ChangeStatusItem(arg.ID, 1);
            await LoadBooks();
        }
        /// <summary>
        /// Changes status on Book from 1(active) to 0(inactive)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task RemoveBookCommandMethod(Item item)
        {
            if (string.IsNullOrEmpty(item.reasonToDelete) || string.IsNullOrWhiteSpace(item.reasonToDelete))
            {
                MessageBox.Show("Fyll i anledning!");
                item.reasonToDelete = "";
                await LoadBooks();
                this.NotifyPropertyChanged(nameof(item.reasonToDelete));
                return;
            }
            this.NotifyPropertyChanged(nameof(item.reasonToDelete));
            await itemRepo.ChangeStatusItem(item.ID);
            await itemRepo.DeleteReason(item);
            await LoadBooks();
        }

        /// <summary>
        /// Updates a Book in DB
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public async Task UpdateBookCommandMethod(Item item)
        {
            // Retrieve the stored index
            if (SelectedAuthorIndex >= 0)
            {
                item.ref_author_id = Authors[SelectedAuthorIndex].author_id;
                // reset
                SelectedAuthorIndex = -1;
            }

            // Dapper does not like uninvited variables
            item.Author = null;
            await itemRepo.Update(item);
            await LoadBooks();
        }
        /// <summary>
        /// Gets the books Category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task GetBookCategory(string category)
        {
            // Check if category is correct length (1 or 2)
            if (category.Length > 2)
            {
                MessageBox.Show("Max två tecken i Kategori");
                InputCategory = "";
                return;
            }

            // Check if valid A-Z combination input
            if (!new Regex(@"^[A-Za-z]{1,2}$").Match(category).Success)
            {
                MessageBox.Show("Endast 1 eller 2 bokstäver för kategori");
                InputCategory = "";
                return;
            }

            BookCategory = await categoryRepo.GetCategory(Converter(category));
            // if GetCategory returns null return error
            if (BookCategory == null)
            {
                MessageBox.Show("Felaktig inmatning i Kategori");
                InputCategory = "";
                return;
            }
            else
                SelectedItem.category = BookCategory.category;

        }

        /// <summary>
        /// Method to clear the lines after you added a book.
        /// </summary>
        /// <returns></returns>
        public async Task ClearBookLines(string sender)
        {
            SelectedItem.title = "";
            SelectedItem.description = "";
            SelectedItem.isbn = "";
            InputCategory = "";
            SelectedItem.url = "";
            SelectedItem.cover = "";
            SelectedItem.reasonToDelete = "";
            SelectedItem.year = 0;
            SelectedAuthor = null;
            SelectedItemIsEBook = false;
            OnPropertyChanged(nameof(SelectedItem));
        }

        private async Task SearchAuthor()
        {

        }


        /// <summary>
        /// Puttin' da file on da server
        /// </summary>
        /// <returns></returns>
        private async Task UploadFile(string arg)
        {
            // Open up file dialog for selection
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            // Multiple files allowed
            dialog.Multiselect = true;

            // List of file paths
            List<string> filenames = new List<string>();

            if (dialog.ShowDialog() == true)
            {
                foreach (string filename in dialog.FileNames.ToList())
                {

                    if (arg != "book_cover" && arg != "book_url" && arg != "ebook_cover" && arg != "ebook_url")
                    {
                        MessageBox.Show("Error, fel input");
                        return;
                    }

                    System.IO.FileInfo info = new System.IO.FileInfo(filename);

                    long len = info.Length;

                    // If exceds 10 MB (mebibyte)
                    if (len > 10490000)
                    {
                        MessageBox.Show("Kan ej ladda upp över 10MB");
                        continue;
                        //throw new Exception("Not so large files plz, todo; display error here instead of exception");
                    }

                    // Upload file to server
                    var JSONresponseObject = await Etc.WebHelper.UploadCoverImage(filename);

                    // Raise failure if error
                    if (!JSONresponseObject.Value<bool>("success"))
                    {
                        MessageBox.Show(JSONresponseObject.Value<string>("message"));
                        continue;
                    }

                    LibrarySystem.Etc.JkbZoneFile parsedObj = new LibrarySystem.Etc.JkbZoneFile(JSONresponseObject.Value<string>("location"));

                    if (arg.Contains("cover"))
                    {
                        SelectedItem.cover = parsedObj.Location;
                        OnPropertyChanged("SelectedItem");
                    }
                    if (arg.Contains("url"))
                    {
                        SelectedItem.url = parsedObj.Location;
                        OnPropertyChanged("SelectedItem");
                    }

                    // Do something with the response
                    MessageBox.Show("Tillagd!");
                }
            }
        }

        /// <summary>
        /// Uppdaterar cover image för en item i item collections
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private async Task UpdateFile(Item item)
        {
            // Open up file dialog for selection
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            // Multiple files allowed
            dialog.Multiselect = true;

            if (dialog.ShowDialog() == true)
            {
                foreach (string filename in dialog.FileNames.ToList())
                {
                    System.IO.FileInfo info = new System.IO.FileInfo(filename);
                    long len = info.Length;

                    // If exceds 10 MB (mebibyte)
                    if (len > 10490000)
                    {
                        MessageBox.Show("Kan ej ladda upp över 10MB");
                        continue;
                    }

                    // Upload file to server
                    var JSONresponseObject = await Etc.WebHelper.UploadCoverImage(filename);

                    // Raise failure if error
                    if (!JSONresponseObject.Value<bool>("success"))
                    {
                        MessageBox.Show(JSONresponseObject.Value<string>("message"));
                        continue;
                    }

                    LibrarySystem.Etc.JkbZoneFile parsedObj = new LibrarySystem.Etc.JkbZoneFile(JSONresponseObject.Value<string>("location"));

                    item.cover = parsedObj.Location;

                }
            }
        }
        private async Task UpdateUrl(Item item)
        {
            // Open up file dialog for selection
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            // Multiple files allowed
            dialog.Multiselect = true;

            if (dialog.ShowDialog() == true)
            {
                foreach (string filename in dialog.FileNames.ToList())
                {
                    System.IO.FileInfo info = new System.IO.FileInfo(filename);
                    long len = info.Length;

                    // If exceds 10 MB (mebibyte)
                    if (len > 10490000)
                    {
                        MessageBox.Show("Kan ej ladda upp över 10MB");
                        continue;
                    }

                    // Upload file to server
                    var JSONresponseObject = await Etc.WebHelper.UploadCoverImage(filename);

                    // Raise failure if error
                    if (!JSONresponseObject.Value<bool>("success"))
                    {
                        MessageBox.Show(JSONresponseObject.Value<string>("message"));
                        continue;
                    }

                    LibrarySystem.Etc.JkbZoneFile parsedObj = new LibrarySystem.Etc.JkbZoneFile(JSONresponseObject.Value<string>("location"));

                    item.url = parsedObj.Location;

                }
            }
        }

        /// <summary>
        /// Converts first to upper and last to lower
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public string Converter(string category)
        {
            string first, last, cat;
            first = category.First().ToString();
            if (category.Length > 1)
            {
                last = category.Last().ToString();
                return cat = first.ToUpper() + last.ToLower();
            }
            else
                return cat = first.ToUpper();
        }

        private async void ReloadBooksAsync() => await LoadBooks();
        public async void LoadDataAsync()
        {
            await LoadAuthors();
            await LoadBooks();

        }

        private async void ReloadAuthorsAsync() => await LoadAuthors();

        /// <summary>
        /// Load books from DB via SQL, default limit amount is set to 5 unless specified otherwise
        /// </summary>
        /// <returns></returns>
        public async Task LoadBooks() =>  await LoadBooks(LimitBookFilter ? 9999999  : 10);

        public async Task LoadBooks(int limiter)
        {
            //var watch = System.Diagnostics.Stopwatch.StartNew();
            //List<Task> tasks = new List<Task>();
            Items.Clear();
            foreach (var item in await itemRepo.ReadAllItemsWithStatus(ActiveFilter, limiter))
            {
                //tasks.Add(Items.Add(item));
                Items.Add(item);
            }
            //watch.Stop();
            //var result = watch.ElapsedMilliseconds;
            //MessageBox.Show((result).ToString());
        }

        /// <summary>Reloads all the Authors from DB</summary>
        public async Task LoadAuthors()
        {
            Authors.Clear();

            foreach (Author author in await authorRepo.Search(addBookAuthorSearch))
            {
                Authors.Add(author);
            }
        }

        // Generic page for reports
        public void GoToReportPage()
        {
            var reports = new ReportsView();
            reports.DataContext = new ReportsViewModel();
            reports.ShowDialog();
        }

        public async Task BookReportMethod()
        {
            var bookreport = new BookReportView();
            bookreport.DataContext = new BookReportViewModel();
            bookreport.ShowDialog();
        }

        //
        // Expiremental stuff below here
        //

        private RelayCommand DoWorkCommand { get; set; }

        public async Task DoWork(IProgress<int> progress = null)
        {
            await Task.Run(() =>
            {
                for (int i = 1; i < 11; i++)
                {
                    var count = 0;
                    for (int j = 0; j < 10000000; j++)
                    {
                        count += j;
                    }
                    progress.Report(i * 10);
                }
            });
        }
    }
}
