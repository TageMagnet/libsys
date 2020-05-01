using Library;
using LibrarySystem.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.ViewModels
{
    public class LibrarianViewModel
    {
        #region Properties
        public BookRepository bookRepo = new BookRepository();
        public eBookRepository eBookRepo = new eBookRepository();
        public EventRepository eventRepo = new EventRepository();

        public string Author { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public double Completion { get; set; }

        public List<Event> ListOfEvents { get; set; }
        public List<Book> Books { get; set; }
        public List<eBook> eBooks { get; set; }
        public List<Event> Event { get; set; }

        #endregion

        #region Commands
        public ReactiveCommand<Unit,Unit> AddBookCommand { get; set; }
        public ReactiveCommand<Unit, Unit> AddeBookCommand { get; set; }
        public ReactiveCommand<Unit, Unit> AddEventCommand { get; set; }

        #endregion
        public LibrarianViewModel()
        {
            AddBookCommand = ReactiveCommand.CreateFromTask(() => AddBookCommandMethod());
            AddeBookCommand = ReactiveCommand.CreateFromTask(() => AddeBookCommandMethod());
            AddEventCommand = ReactiveCommand.CreateFromTask(() => AddEventCommandMethod());
            LoadDataAsync();
        }

        public async Task AddBookCommandMethod()
        {

        }

        public async Task AddeBookCommandMethod()
        {

        }
        public async Task AddEventCommandMethod()
        {

        }

        public async void LoadDataAsync()
        {
            await LoadBooks();
            await LoadEbooks();
            await LoadEvents();
        }

        // Hämtar hem böcker
        public async Task LoadBooks()
        {
            Books = new List<Book>(await bookRepo.ReadAll());
        }
        // Hämtar hem e-böcker
        public async Task LoadEbooks()
        {
            eBooks = new List<eBook>(await eBookRepo.ReadAll());
        }
        // Hämtar hem events
        public async Task LoadEvents()
        {
            Event = new List<Event>(await eventRepo.ReadAll());
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
