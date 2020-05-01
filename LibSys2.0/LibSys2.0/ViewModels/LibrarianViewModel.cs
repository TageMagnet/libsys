using Library;
using LibrarySystem.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LibrarySystem.ViewModels
{
    public class LibrarianViewModel
    {
        #region Properties
        public BookRepository bookRepo = new BookRepository();
        public eBookRepository eBookRepo = new eBookRepository();
        public EventRepository eventRepo = new EventRepository();

        public Book SelectedBook { get; set; }

        public List<Event> ListOfEvents { get; set; }
        public ObservableCollection<Book> Books { get; set; }
        public ObservableCollection<eBook> eBooks { get; set; }
        public ObservableCollection<Event> Events { get; set; }

        #endregion

        #region Commands
        public ReactiveCommand<Unit,Unit> AddBookCommand { get; set; }
        public ReactiveCommand<int, Unit> RemoveBookCommand { get; set; }
        public ReactiveCommand<Unit, Unit> AddeBookCommand { get; set; }
        public ReactiveCommand<Unit, Unit> AddEventCommand { get; set; }

        #endregion
        public LibrarianViewModel()
        {
            SelectedBook = new Book();
            Books = new ObservableCollection<Book>();
            eBooks = new ObservableCollection<eBook>();
            Events = new ObservableCollection<Event>();

            AddBookCommand = ReactiveCommand.CreateFromTask(() => AddBookCommandMethod());
            RemoveBookCommand = ReactiveCommand.CreateFromTask((int id) => RemoveBookCommandMethod(id));
            AddeBookCommand = ReactiveCommand.CreateFromTask(() => AddeBookCommandMethod());
            AddEventCommand = ReactiveCommand.CreateFromTask(() => AddEventCommandMethod());
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
            await bookRepo.Create(SelectedBook);
            await LoadBooks();

        }
        public async Task RemoveBookCommandMethod(int id)
        {
            await bookRepo.Delete(id);
            await LoadBooks();
        }

        public async Task AddeBookCommandMethod()
        {

        }
        public async Task AddEventCommandMethod()
        {

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
