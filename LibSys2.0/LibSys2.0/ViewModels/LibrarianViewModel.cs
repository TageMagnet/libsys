﻿using Library;
using LibrarySystem.Models;
using Org.BouncyCastle.Bcpg;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Text;
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

        public BookRepository bookRepo = new BookRepository();
        public eBookRepository eBookRepo = new eBookRepository();
        public EventRepository eventRepo = new EventRepository();
        public AuthorRepository authorRepo = new AuthorRepository();
        public MemberRepository memberRepo = new MemberRepository();
        public Book SelectedBook { get; set; } = new Book();
        public eBook SelectedeBook { get; set; } = new eBook();

        public Author SelectedAuthor { get; set; } = new Author();
        public Member SelectedMember { get; set; } = new Member();
        public string ReasonToDelete { get; set; }

        public ObservableCollection<Book> Books { get; set; } = new ObservableCollection<Book>();
        public ObservableCollection<eBook> eBooks { get; set; } = new ObservableCollection<eBook>();
        public ObservableCollection<Event> Events { get; set; } = new ObservableCollection<Event>();
        public ObservableCollection<Author> Authors { get; set; } = new ObservableCollection<Author>();
        /// <summary>Index storage when updating</summary>
        public int SelectedAuthorIndex { get; set; } = -1;
        public ObservableCollection<Member> Members { get; set; } = new ObservableCollection<Member>();

        public ObservableCollection<string> AvailableRoles { get; set; } = new ObservableCollection<string>() { "Admin", "Bibliotekarie", "Besökare" };

        #endregion

        #region Commands
        public ReactiveCommand<Unit, Unit> AddBookCommand { get; set; }

        public ReactiveCommand<Book, Unit> UpdateBookCommand { get; set; }
        public ReactiveCommand<eBook, Unit> UpdateeBookCommand { get; set; }
        public ReactiveCommand<Author, Unit> UpdateAuthorCommand { get; set; }
        public ReactiveCommand<int, Unit> RemoveBookCommand { get; set; }
        public ReactiveCommand<int, Unit> RemoveeBookCommand { get; set; }
        public ReactiveCommand<int, Unit> RemoveAuthorCommand { get; set; }
        public ReactiveCommand<Unit, Unit> AddeBookCommand { get; set; }
        public ReactiveCommand<Unit, Unit> AddEventCommand { get; set; }
        public ReactiveCommand<object, Unit> ToggleHidden { get; set; }
        public ReactiveCommand<object, Unit> ToggleVisible { get; set; }

        public ReactiveCommand<Unit, Unit> AddAuthorCommand { get; set; }
        public ReactiveCommand<Unit, Unit> AddNewMember { get; set; }
        public ReactiveCommand<object, Unit> DeleteMemberCommand { get; set; }

        public ReactiveCommand<Member, Unit> UpdateMemberCommand { get; set; }

        public ReactiveCommand<string, Unit> FileUploadCommand { get; set; }

        #endregion
        public LibrarianViewModel()
        {

            AddBookCommand = ReactiveCommand.CreateFromTask(() => AddBookCommandMethod());
            UpdateBookCommand = ReactiveCommand.CreateFromTask((Book book) => UpdateBookCommandMethod(book));
            UpdateeBookCommand = ReactiveCommand.CreateFromTask((eBook ebook) => UpdateeBookCommandMethod(ebook));
            RemoveBookCommand = ReactiveCommand.CreateFromTask((int id) => RemoveBookCommandMethod(id));
            RemoveeBookCommand = ReactiveCommand.CreateFromTask((int id) => RemoveeBookCommandMethod(id));
            RemoveAuthorCommand = ReactiveCommand.CreateFromTask((int id) => RemoveAuthorCommandMethod(id));
            AddeBookCommand = ReactiveCommand.CreateFromTask(() => AddeBookCommandMethod());
            AddEventCommand = ReactiveCommand.CreateFromTask(() => AddEventCommandMethod());
            ToggleHidden = ReactiveCommand.CreateFromTask((object param) => HiddenCommandMethod(param));
            ToggleVisible = ReactiveCommand.CreateFromTask((object param) => VisibleCommandMethod(param));
            AddAuthorCommand = ReactiveCommand.CreateFromTask(() => AddAuthorCommandMethod());
            UpdateAuthorCommand = ReactiveCommand.CreateFromTask((Author author) => UpdateAuthorCommandMethod(author));

            AddNewMember = ReactiveCommand.CreateFromTask(() => AddNewMemberCommand());
            DeleteMemberCommand = ReactiveCommand.CreateFromTask((object obj) => DeleteMemberCommandMethod(obj));
            UpdateMemberCommand = ReactiveCommand.CreateFromTask((Member member) => UpdateMemberCommandMethod(member));

            FileUploadCommand = ReactiveCommand.CreateFromTask((string arg) => UploadFile(arg));

            LoadDataAsync();
        }

        /// <summary>
        /// Checks and adds a Book to DB
        /// </summary>
        /// <returns></returns>
        public async Task AddBookCommandMethod()
        #region ...
        {
            if (SelectedBook.title == null)
            {
                MessageBox.Show("Lägg till titel!");
                return;
            }
            if (SelectedAuthor.author_id == 0)
            {
                MessageBox.Show("Lägg till författare!");
                return;
            }
            if (SelectedBook.description == null)
            {
                MessageBox.Show("Lägg till beskrivning!");
                return;
            }
            if (SelectedBook.isbn == null)
            {
                MessageBox.Show("Lägg till isbn!");
                return;
            }
            if (SelectedBook.category == null)
            {
                MessageBox.Show("Lägg till kategori!");
                return;
            }
            SelectedBook.ref_author_id = SelectedAuthor.author_id;
            await bookRepo.Create(SelectedBook);
            await LoadBooks();
            await ClearBookLines("books");

        }
        #endregion
        /// <summary>
        /// Checks and adds a E-Book to the database
        /// </summary>
        /// <returns></returns>
        public async Task AddeBookCommandMethod()
        #region ...
        {
            if (SelectedeBook.title == null)
            {
                MessageBox.Show("Lägg till titel!");
                return;
            }
            if (SelectedAuthor.author_id == 0)
            {
                MessageBox.Show("Lägg till författare!");
                return;
            }
            if (SelectedeBook.description == null)
            {
                MessageBox.Show("Lägg till beskrivning!");
                return;
            }
            if (SelectedeBook.isbn == null)
            {
                MessageBox.Show("Lägg till isbn!");
                return;
            }
            if (SelectedeBook.category == null)
            {
                MessageBox.Show("Lägg till kategori!");
                return;
            }
            SelectedeBook.ref_author_id = SelectedAuthor.author_id;
            await eBookRepo.Create(SelectedeBook);
            await LoadeBooks();
            await ClearBookLines("ebooks");
        }
        #endregion


        /// <summary>
        /// Updates a Book in DB
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public async Task UpdateBookCommandMethod(Book book)
        #region ...
        {
            // Retrieve the stored index
            if (SelectedAuthorIndex >= 0)
            {
                book.ref_author_id = Authors[SelectedAuthorIndex].author_id;
                // reset
                SelectedAuthorIndex = -1;
            }

            // Dapper does not like uninvited variables
            book.Author = null;
            await bookRepo.Update(book);
            await LoadBooks();
        }
        #endregion

        public async Task UpdateeBookCommandMethod(eBook ebook)
        #region ...
        {
            if (SelectedAuthorIndex >= 0)
            {
                ebook.ref_author_id = Authors[SelectedAuthorIndex].author_id;
                // reset
                SelectedAuthorIndex = -1;
            }

            ebook.Author = null;
            await eBookRepo.Update(ebook);
            await LoadeBooks();
        }
        #endregion

        /// <summary>
        /// Updates a author to db.
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        public async Task UpdateAuthorCommandMethod(Author author)
        #region ...
        {
            await authorRepo.Update(author);
            await LoadAuthors();
        }
        #endregion

        public async Task UpdateMemberCommandMethod(Member member)
        #region ...
        {
            await memberRepo.Update(member);
            await LoadMembers();

        }
        #endregion

        /// <summary>
        /// Changes status on Book from 1(active) to 0(inactive)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task RemoveBookCommandMethod(int id)
        #region ...
        {
            if (string.IsNullOrEmpty(ReasonToDelete) || string.IsNullOrWhiteSpace(ReasonToDelete))
            {
                MessageBox.Show("Fyll i anledning!");
                ReasonToDelete = "";
                this.NotifyPropertyChanged(nameof(ReasonToDelete));
                return;
            }

            ReasonToDelete = "";
            this.NotifyPropertyChanged(nameof(ReasonToDelete));
            await bookRepo.ChangeStatusBook(id);
            await LoadBooks();
        }
        #endregion

        /// <summary>
        /// Removes/delete a author from DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task RemoveAuthorCommandMethod(int id)
        #region ...
        {
            int numberOfAuthorBooks = 0;

            foreach (var book in Books)
            {
                if (book.ref_author_id == id)
                {
                    numberOfAuthorBooks++;
                }
            }
            foreach (var ebok in eBooks)
            {
                if (ebok.ref_author_id == id)
                {
                    numberOfAuthorBooks++;
                }
            }

            if (numberOfAuthorBooks > 0)
            {
                MessageBox.Show($"Denna författare är bunden till {numberOfAuthorBooks}st böcker. Och kan därför inte tas bort");
                return;
            }

            await authorRepo.Delete(id);
            await LoadAuthors();
        }
        #endregion

        /// <summary>
        /// Changes status on eBook from 1(active) to 0(inactive)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task RemoveeBookCommandMethod(int id)
        #region ...
        {
            if (string.IsNullOrEmpty(ReasonToDelete) || string.IsNullOrWhiteSpace(ReasonToDelete))
            {
                MessageBox.Show("Fyll i anledning!");
                ReasonToDelete = "";
                this.NotifyPropertyChanged(nameof(ReasonToDelete));
                return;
            }
            ReasonToDelete = "";
            this.NotifyPropertyChanged(nameof(ReasonToDelete));
            await eBookRepo.ChangeStatuseBook(id);
            await LoadeBooks();
        }
        #endregion

        public async Task AddEventCommandMethod()
        #region ...
        {

        }
        #endregion


        /// <summary>
        /// Makes Arrow down button Visible
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public async Task VisibleCommandMethod(object arg)
        #region ...
        {
            var button = (Button)arg;
            button.IsEnabled = true;
            ReasonToDelete = "";
            this.OnPropertyChanged(nameof(ReasonToDelete));
        }
        #endregion


        /// <summary>
        /// Makes arrow down button Hidden
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public async Task HiddenCommandMethod(object arg)
        #region ...
        {
            var button = (Button)arg;
            button.IsEnabled = false;
        }
        #endregion


        /// <summary>
        /// Method to add author to DB
        /// </summary>
        /// <returns></returns>
        public async Task AddAuthorCommandMethod()
        #region ...
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
        #endregion


        /// <summary>
        /// Loads all the data from DB
        /// </summary>
        public async void LoadDataAsync()
        #region ...
        {
            await LoadBooks();
            await LoadeBooks();
            await LoadEvents();
            await LoadAuthors();
        }
        #endregion


        /// <summary>
        /// Reloads books from DB
        /// </summary>
        /// <returns></returns>
        public async Task LoadBooks()
        #region ...
        {
            Books.Clear();
            foreach (var book in await bookRepo.ReadAllActiveBooks())
            {
                Books.Add(book);
            }
        }

        #endregion


        /// <summary>
        /// Reloads E-Books From DB
        /// </summary>
        /// <returns></returns>
        public async Task LoadeBooks()
        #region ...
        {
            eBooks.Clear();
            foreach (var ebook in await eBookRepo.ReadAllActiveeBooks())
            {
                eBooks.Add(ebook);
            }
        }
        #endregion


        /// <summary>
        /// Reloads all the Events from DB
        /// </summary>
        /// <returns></returns>
        public async Task LoadEvents()
        #region ...
        {
            Events.Clear();
            foreach (var _event in await eventRepo.ReadAll())
            {
                Events.Add(_event);
            }
        }
        #endregion


        /// <summary>
        /// Reloads all the Authors from DB
        /// </summary>
        /// <returns></returns>
        public async Task LoadAuthors()
        #region ...
        {
            Authors.Clear();
            foreach (var author in await authorRepo.ReadAll())
            {
                Authors.Add(author);
            }
        }
        #endregion

        public async Task LoadMembers()
        {
            Members.Clear();

            foreach (Member member in await memberRepo.ReadAllActive())
            {
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
            await memberRepo.Create(NewMember);
            await LoadMembers();
            await ClearMemberLines("members");
        }

        public async Task DeleteMemberCommandMethod(object obj)
        {
            Member member = (Member)obj;
            member.is_active = 0;
            await memberRepo.Update(member);
            await LoadMembers();
        }

        public async Task ClearMemberLines(string sender)
        {
            //Clear Members
            NewMember.email = "";
            NewMember.nickname = "";
            NewMember.pwd = "";
            NewMember.role = "";
            this.OnPropertyChanged(nameof(NewMember));

        }

        /// <summary>
        /// Method to clear the lines after you added a book.
        /// </summary>
        /// <returns></returns>
        public async Task ClearBookLines(string sender)
        #region ...
        {

            //Clear Books
            if (sender == "books")
            {
                SelectedBook.title = "";
                SelectedBook.description = "";
                SelectedBook.isbn = "";
                SelectedBook.category = "";
                this.OnPropertyChanged(nameof(SelectedBook));

            }

            //Clear E-Books
            else if (sender == "ebooks")
            {
                SelectedeBook.title = "";
                SelectedeBook.description = "";
                SelectedeBook.isbn = "";
                SelectedeBook.category = "";
                this.OnPropertyChanged(nameof(SelectedeBook));
            }
        }
        #endregion

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
                foreach(string filename in dialog.FileNames.ToList())
                {

                    if(arg != "book_cover" && arg != "book_url")
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

                    if (arg == "book_cover")
                    {
                        SelectedBook.cover = JSONresponseObject.Value<string>("location");
                    }
                    else if (arg == "book_url")
                    {
                        SelectedBook.url = JSONresponseObject.Value<string>("location");
                    }
                    OnPropertyChanged("SelectedBook");


                    // Do something with the response
                    MessageBox.Show("Laddade upp! Pleäse tryck på uppdatera nu");
                }
            }
        }
    }
}
