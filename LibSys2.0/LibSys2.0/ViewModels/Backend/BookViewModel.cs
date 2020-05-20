using Library;
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

        public string ReasonToDelete { get; set; }
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
        public int ActiveFilter { get; set; } = 1;
        public RelayCommand AddBookCommand { get; set; }
        public RelayCommandWithParameters UpdateBookCommand { get; set; } // item
        public RelayCommandWithParameters RemoveBookCommand { get; set; }
        public RelayCommandWithParameters ActivateBookCommand { get; set; }
        public RelayCommandWithParameters ToggleHidden { get; set; }
        public RelayCommandWithParameters ToggleVisible { get; set; }

        public BookViewModel()
        {
            AddBookCommand = new RelayCommand(async() => await AddBookCommandMethod());
            UpdateBookCommand = new RelayCommandWithParameters(async (param) => await UpdateBookCommandMethod((Item)param));
            RemoveBookCommand = new RelayCommandWithParameters(async (param) => await RemoveBookCommandMethod((int)param));
            ActivateBookCommand = new RelayCommandWithParameters(async(param) => await ActivateBook((Item)param));
            ToggleHidden = new RelayCommandWithParameters(async (param) => await HiddenCommandMethod((Button)param));
            ToggleVisible = new RelayCommandWithParameters(async (param) => await VisibleCommandMethod((Button)param));
            LoadDataAsync();
        }

        /// <summary>Makes Arrow down button Visible</summary>
        /// <param name="arg"></param>
        private async Task VisibleCommandMethod(Button arg)
        {
            arg.IsEnabled = true;
            ReasonToDelete = "";
            this.OnPropertyChanged(nameof(ReasonToDelete));
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
            SelectedItem.ref_author_id = SelectedAuthor.author_id;
            SelectedItem.is_active = 1;
            await GetBookCategory(InputCategory);
            if (SelectedItem.category == null)
            {
                return;
            }
            await itemRepo.Create(SelectedItem);
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
        public async Task RemoveBookCommandMethod(int id)
        {
            if (string.IsNullOrEmpty(ReasonToDelete) || string.IsNullOrWhiteSpace(ReasonToDelete))
            {
                MessageBox.Show("Fyll i anledning!");
                ReasonToDelete = "";
                await LoadBooks();
                this.NotifyPropertyChanged(nameof(ReasonToDelete));
                return;
            }

            ReasonToDelete = "";
            this.NotifyPropertyChanged(nameof(ReasonToDelete));
            await itemRepo.ChangeStatusItem(id);
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
            SelectedItem.year = 0;
            SelectedAuthor = null;
            OnPropertyChanged(nameof(SelectedItem));
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
                    MessageBox.Show("Laddade upp! Pleäse tryck på uppdatera nu");
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
        public async void LoadDataAsync()
        {
            await LoadAuthors();
            await LoadBooks();
        }

        /// <summary>
        /// Reloads books from DB
        /// </summary>
        /// <returns></returns>
        public async Task LoadBooks()
        {

            Items.Clear();
            foreach (var item in await itemRepo.ReadAllItemsWithStatus(ActiveFilter))
            {
                Items.Add(item);
            }
        }

        /// <summary>Reloads all the Authors from DB</summary>
        public async Task LoadAuthors()
        {
            Authors.Clear();
            foreach (var author in await authorRepo.ReadAll())
            {
                Authors.Add(author);
            }
        }
    }
}
