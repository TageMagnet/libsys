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

        public ReactiveCommand<Book, Unit> UpdateBook { get; set; }
        public ReactiveCommand<int, Unit> RemoveBookCommand { get; set; }
        public ReactiveCommand<Unit, Unit> AddeBookCommand { get; set; }
        public ReactiveCommand<Unit, Unit> AddEventCommand { get; set; }
        public ReactiveCommand<object, Unit> ToggleHidden { get; set; }

        #endregion
        public LibrarianViewModel()
        {
            SelectedBook = new Book();
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
            await bookRepo.Create(SelectedBook);
            await LoadBooks();

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
            await bookRepo.Delete(id);
            await LoadBooks();
        } 
        #endregion
        public async Task AddeBookCommandMethod()
        #region ...
        {

        } 
        #endregion
        public async Task AddEventCommandMethod()
        #region ...
        {

        } 
        #endregion
        public async Task ToggleHiddenCommandMethod(object arg)
        #region ...
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
