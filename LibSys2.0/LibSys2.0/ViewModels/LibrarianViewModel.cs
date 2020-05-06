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
            ToggleHidden = ReactiveCommand.CreateFromTask((object param) => ToggleHiddenCommandMethod(param));
            LoadDataAsync();
        }

        #region Command Methods
        public async Task AddBookCommandMethod()
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

        public async Task UpdateBookCommandMethod(Book book)
        {
            await bookRepo.Update(book);
            await LoadBooks();
        }
        public async Task RemoveBookCommandMethod(int id)
        {
            await bookRepo.Delete(id);
            await LoadBooks();
        }
        /// <summary>
        /// Checks and adds a E-Book to the database
        /// </summary>
        /// <returns></returns>
        public async Task AddeBookCommandMethod()
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
        public async Task AddEventCommandMethod()
        {

        }

        public async Task ToggleHiddenCommandMethod(object arg)
        {
            var button = (Button)arg;
            button.IsEnabled = button.IsEnabled ? false : true;
        } 
        #endregion
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
