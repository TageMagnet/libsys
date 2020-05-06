using Library;
using LibrarySystem.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LibrarySystem.ViewModels
{
    public class LibrarianViewModel : BaseViewModel
    {
        #region Properties
        public BookRepository bookRepo = new BookRepository();
        public eBookRepository eBookRepo = new eBookRepository();
        public EventRepository eventRepo = new EventRepository();
        public Book SelectedBook { get; set; }
        public eBook SelectedeBook { get; set; }

        public string ReasonToDelete { get; set; }

        public List<Event> ListOfEvents { get; set; }
        public ObservableCollection<Book> Books { get; set; }
        public ObservableCollection<eBook> eBooks { get; set; }
        public ObservableCollection<Event> Events { get; set; }

        #endregion

        #region Commands
        public ReactiveCommand<Unit,Unit> AddBookCommand { get; set; }

        public ReactiveCommand<Book, Unit> UpdateBook { get; set; }
        public ReactiveCommand<int, Unit> RemoveBookCommand { get; set; }
        public ReactiveCommand<Unit, Unit> AddeBookCommand { get; set; }
        public ReactiveCommand<Unit, Unit> AddEventCommand { get; set; }
        public ReactiveCommand<object, Unit> ToggleHidden { get; set; }
        public ReactiveCommand<object, Unit> ToggleVisible { get; set; }


        #endregion
        public LibrarianViewModel()
        {
            SelectedBook = new Book();
            SelectedeBook = new eBook();
            Books = new ObservableCollection<Book>();
            eBooks = new ObservableCollection<eBook>();
            Events = new ObservableCollection<Event>();

            AddBookCommand = ReactiveCommand.CreateFromTask(() => AddBookCommandMethod());
            UpdateBook = ReactiveCommand.CreateFromTask((Book book) => UpdateBookCommandMethod(book));
            RemoveBookCommand = ReactiveCommand.CreateFromTask((int id) => RemoveBookCommandMethod(id));
            AddeBookCommand = ReactiveCommand.CreateFromTask(() => AddeBookCommandMethod());
            AddEventCommand = ReactiveCommand.CreateFromTask(() => AddEventCommandMethod());
            ToggleHidden = ReactiveCommand.CreateFromTask((object param) => HiddenCommandMethod(param));
            ToggleVisible = ReactiveCommand.CreateFromTask((object param) => VisibleCommandMethod(param));
            LoadDataAsync();
        }

        public async Task AddBookCommandMethod()
        #region ...
        {
            if (SelectedBook.title == null)
            {
                MessageBox.Show("Lägg till titel!");
                return;
            }
            //if (SelectedBook.author == null)
            //{
            //    MessageBox.Show("Lägg till författare!");
            //    return;
            //}
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
            await bookRepo.Create(SelectedBook);
            await LoadBooks();
            await ClearBookLines("books");

        }
        #endregion
        public async Task UpdateBookCommandMethod(Book book)
        #region ...
        {
            await bookRepo.Update(book);
            await LoadBooks();
        }
        #endregion
        public async Task RemoveBookCommandMethod(int id)
        #region ...
        {
            if(ReasonToDelete == null)
            {
                MessageBox.Show("Fyll i anledning!");
                return;
            }
            await bookRepo.Delete(id);
            await LoadBooks();
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
            //if (SelectedBook.author == null)
            //{
            //    MessageBox.Show("Lägg till författare!");
            //    return;
            //}
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
            await eBookRepo.Create(SelectedeBook);
            await LoadEbooks();
            await ClearBookLines("ebooks");
        }
        #endregion
        public async Task AddEventCommandMethod()
        #region ...
        {

        }


        public async Task VisibleCommandMethod(object arg)
        {
            var button = (Button)arg;
            button.IsEnabled = true;
            ReasonToDelete = "";
            this.OnPropertyChanged(nameof(ReasonToDelete));
        }
        public async Task HiddenCommandMethod(object arg)
        {
            var button = (Button)arg;
            button.IsEnabled = false;
        }


        #endregion
        //public async Task ToggleHiddenCommandMethod(object arg)
        
        //{
        //    var button = (Button)arg;
        //    button.IsEnabled = button.IsEnabled ? false : true;
        //}

        public async void LoadDataAsync()
        {
            await LoadBooks();
            await LoadEbooks();
            await LoadEvents();
        }

        // Hämtar hem böcker
        public async Task LoadBooks()
        {
            Books.Clear();
            foreach(var book in await bookRepo.ReadAll())
            {
                Books.Add(book);
            }
            //Books.Add( await bookRepo.ReadAll());

        }
        // Hämtar hem e-böcker
        public async Task LoadEbooks()
        {
            eBooks.Clear();
            foreach (var ebook in await eBookRepo.ReadAll())
            {
                eBooks.Add(ebook);
            }
        }
        // Hämtar hem events
        public async Task LoadEvents()
        {
            Events.Clear();
            foreach (var _event in await eventRepo.ReadAll())
            {
                Events.Add(_event);
            }
        }
        /// <summary>
        /// Method to clear the lines after you added a book.
        /// </summary>
        /// <returns></returns>
        public async Task ClearBookLines(string sender)
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
        //private ObservableCollection<Event> events;
        //public ObservableCollection<Event> Events
        //{
        //    get { return events; }
        //    set
        //    {
        //        var eventIE = repo.ReadEvents();
        //        events = new ObservableCollection<Event>(eventIE.ToList());
        //    }
        //}
        
    }
}
