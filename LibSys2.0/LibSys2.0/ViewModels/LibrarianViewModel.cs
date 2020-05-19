using Library;
using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MessageBox = System.Windows.MessageBox;

namespace LibrarySystem.ViewModels
{
    public class LibrarianViewModel : BaseViewModel
    {
        #region Properties

        // Private holder
        private int tabControlSelectedIndex { get; set; } = 0;

        /// <summary>
        /// Trigggers when tab item is changed
        /// </summary>
        public int TabControlSelectedIndex
        {
            get
            {
                return tabControlSelectedIndex;
            }
            set
            {
                tabControlSelectedIndex = value;
                OnPropertyChanged("TabControlSelectedIndex");

                switch ((int)value)
                {
                    case 3:
                        NewMember = new Member();
                        // Ladda members när tab control bytts till rätt sida
                        _ = LoadMembers();
                        break;
                }
            }
        }


        public Member NewMember { get; set; }
        private ItemRepository itemRepo = new ItemRepository();
        public EventRepository eventRepo = new EventRepository();
        public AuthorRepository authorRepo = new AuthorRepository();
        public MemberRepository memberRepo = new MemberRepository();
        public CategoryRepository categoryRepo = new CategoryRepository();
        public Item SelectedItem { get; set; } = new Item();
        public Author SelectedAuthor { get; set; } = new Author();
        public Category BookCategory { get; set; } = new Category();
        public Member SelectedMember { get; set; } = new Member();
        public string ReasonToDelete { get; set; }
        public string InputCategory { get; set; }

        public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();
        public ObservableCollection<Event> Events { get; set; } = new ObservableCollection<Event>();
        public ObservableCollection<Author> Authors { get; set; } = new ObservableCollection<Author>();
        /// <summary>Index storage when updating</summary>
        public int SelectedAuthorIndex { get; set; } = -1;
        public int SelectedRoleIndex { get; set; }
        public ObservableCollection<Member> Members { get; set; } = new ObservableCollection<Member>();

        public ObservableCollection<string> AvailableRoles { get; set; } = new ObservableCollection<string>() { "admin", "librarian", "user", "guest" };

        private bool activeBookFilter = false;

        public bool ActiveBookFilter
        {
            get { return activeBookFilter; }
            set
            {
                activeBookFilter = value;
                if (value == true)
                    ActiveFilter = 0;
                else
                    ActiveFilter = 1;

                LoadAllBooks();
            }
        }

        public int ActiveFilter { get; set; } = 1;
        #endregion

        #region Commands
        public RelayCommand AddBookCommand { get; set; }
        public RelayCommandWithParameters UpdateBookCommand { get; set; }
        public RelayCommandWithParameters UpdateAuthorCommand { get; set; }
        public RelayCommandWithParameters RemoveBookCommand { get; set; }
        public RelayCommandWithParameters RemoveAuthorCommand { get; set; }
        public RelayCommand AddEventCommand { get; set; }
        public RelayCommandWithParameters ToggleHidden { get; set; }
        public RelayCommandWithParameters ToggleVisible { get; set; }
        public RelayCommand AddAuthorCommand { get; set; }
        public RelayCommand AddNewMember { get; set; }
        public RelayCommandWithParameters DeleteMemberCommand { get; set; }
        public RelayCommandWithParameters UpdateMemberCommand { get; set; }
        public RelayCommandWithParameters FileUploadCommand { get; set; }

        #endregion
        public LibrarianViewModel()
        {

            AddBookCommand = new RelayCommand(async () => await AddBookCommandMethod());
            UpdateBookCommand = new RelayCommandWithParameters(async (param) => await UpdateBookCommandMethod((Item)param));
            RemoveBookCommand = new RelayCommandWithParameters(async (param) => await RemoveBookCommandMethod((int)param));

            FileUploadCommand = new RelayCommandWithParameters(async (param) => await UploadFile((string)param));

            ToggleHidden = new RelayCommandWithParameters(async (param) => await HiddenCommandMethod((object)param));
            ToggleVisible = new RelayCommandWithParameters(async (param) => await VisibleCommandMethod((object)param));

            AddAuthorCommand = new RelayCommand(async () => await AddAuthorCommandMethod());
            UpdateAuthorCommand = new RelayCommandWithParameters(async (param) => await UpdateAuthorCommandMethod((Author)param));
            RemoveAuthorCommand = new RelayCommandWithParameters(async (param) => await RemoveAuthorCommandMethod((int)param));

            AddEventCommand = new RelayCommand(async () => await AddEventCommandMethod());

            AddNewMember = new RelayCommand(async() => await AddNewMemberCommand());
            DeleteMemberCommand = new RelayCommandWithParameters(async (param) => await DeleteMemberCommandMethod(param));
            UpdateMemberCommand = new RelayCommandWithParameters(async (param) => await UpdateMemberCommandMethod((Member)param));



            LoadDataAsync();
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

        /// <summary>
        /// Updates a author to db.
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        public async Task UpdateAuthorCommandMethod(Author author)
        {
            await authorRepo.Update(author);
            await LoadAuthors();
        }

        public async Task UpdateMemberCommandMethod(Member member)
        {
            SelectedRoleIndex = AvailableRoles.IndexOf(member.role);
            member.ref_member_role_id++;
            await memberRepo.Update(member);
            await LoadMembers();

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
        /// Removes/delete a author from DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task RemoveAuthorCommandMethod(int id)
        {
            int numberOfAuthorBooks = 0;

            foreach (var item in await itemRepo.ReadAll())
            {
                if (item.ref_author_id == id)
                    numberOfAuthorBooks++;
            }

            if (numberOfAuthorBooks > 0)
            {
                MessageBox.Show($"Denna författare är bunden till {numberOfAuthorBooks}st böcker. Och kan därför inte tas bort");
                await LoadAuthors();
                return;
            }
            try
            {
                await authorRepo.Delete(id);
            }
            catch (Exception)
            {
                MessageBox.Show("Författaren är bunden till en bok. Går ej att ta bort.");
            }
            finally
            {
                await LoadAuthors();
            }

        }

        public async Task AddEventCommandMethod()
        {

        }

        /// <summary>
        /// Makes Arrow down button Visible
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public async Task VisibleCommandMethod(object arg)
        {
            var button = (Button)arg;
            button.IsEnabled = true;
            ReasonToDelete = "";
            this.OnPropertyChanged(nameof(ReasonToDelete));
        }

        /// <summary>
        /// Makes arrow down button Hidden
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public async Task HiddenCommandMethod(object arg)
        {
            var button = (Button)arg;
            button.IsEnabled = false;
        }


        /// <summary>
        /// Method to add author to DB
        /// </summary>
        /// <returns></returns>
        public async Task AddAuthorCommandMethod()
        {
            if (SelectedAuthor.firstname == null)
            {
                MessageBox.Show("Lägg till Förnamn!");
                return;
            }
            if (SelectedAuthor.surname == null)
            {
                MessageBox.Show("Lägg till efternamn!");
                return;
            }
            if (SelectedAuthor.nickname == null)
            {
                MessageBox.Show("Lägg till smeknamn!");
                return;
            }

            await authorRepo.Create(SelectedAuthor);
            await LoadAuthors();
        }


        /// <summary>
        /// Loads all the data from DB
        /// </summary>
        public async void LoadDataAsync()
        {
            await LoadBooks();
            //await LoadeBooks();
            await LoadEvents();
            await LoadAuthors();
        }

        public async void LoadAllBooks()
        {
            await LoadBooks();
            //await LoadeBooks();
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

        /// <summary>
        /// Reloads all the Events from DB
        /// </summary>
        /// <returns></returns>
        public async Task LoadEvents()
        {
            Events.Clear();
            foreach (var _event in await eventRepo.ReadAll())
            {
                Events.Add(_event);
            }
        }


        /// <summary>
        /// Reloads all the Authors from DB
        /// </summary>
        /// <returns></returns>
        public async Task LoadAuthors()
        {
            Authors.Clear();
            foreach (var author in await authorRepo.ReadAll())
            {
                Authors.Add(author);
            }
        }

        public async Task LoadMembers()
        {
            Members.Clear();

            foreach (Member member in await memberRepo.ReadAllActive())
            {
                member.ref_member_role_id--;
                Members.Add(member);
            }
        }

        public async Task AddNewMemberCommand()
        {
            if (NewMember.email == null)
            {
                MessageBox.Show("Lägg till E-Post");
                return;
            }

            if (NewMember.nickname == null)
            {
                MessageBox.Show("Lägg till smeknamn");
                return;
            }
            if (NewMember.pwd == null)
            {
                MessageBox.Show("Lägg till lösenord");
                return;
            }
            if (NewMember.role == null)
            {
                MessageBox.Show("Lägg till roll");
                return;
            }

            // Timestamp, since now is creation date
            NewMember.created_at = DateTime.Now;
            NewMember.is_active = 1;
            NewMember.ref_member_role_id = AvailableRoles.IndexOf(NewMember.role);
            NewMember.ref_member_role_id++;
            await memberRepo.Create(NewMember);
            await LoadMembers();
            await ClearMemberLines("members");
        }

        public async Task DeleteMemberCommandMethod(object obj)
        {
            Member member = (Member)obj;
            member.is_active = 0;
            await UpdateMemberCommandMethod(member);
            await LoadMembers();
        }

        public async Task ClearMemberLines(string sender)
        {
            //Clear Members
            NewMember.email = "";
            NewMember.nickname = "";
            NewMember.pwd = "";
            NewMember.role = null;
            this.OnPropertyChanged(nameof(NewMember));

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
            this.OnPropertyChanged(nameof(SelectedItem));
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
    }
}
