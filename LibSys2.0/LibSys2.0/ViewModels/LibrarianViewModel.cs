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
        public BookRepository bookRepo = new BookRepository();
        public eBookRepository eBookRepo = new eBookRepository();
        public EventRepository eventRepo = new EventRepository();
        public string Title { get; set; }
        public double Completion { get; set; }

        public List<Event> ListOfEvents { get; set; }
        public List<Book> Books { get; set; }
        public List<eBook> eBooks { get; set; }
        public List<Event> Event { get; set; }
        public ReactiveCommand<Unit,Unit> AddBook { get; set; }
        public LibrarianViewModel()
        {
            if (Books == null)
            {
                Books = new List<Book>();
            }
            
        }

        public async void LoadDataAsync()
        {

        }

        public async Task LoadBooks()
        {
            Books = new List<Book>(await bookRepo.ReadAll());
        }
        public async Task LoadEbooks()
        {
            eBooks = new List<eBook>(await eBookRepo.ReadAll());
        }
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
