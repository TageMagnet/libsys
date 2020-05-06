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

        public ReactiveCommand<Book, Unit> UpdateBookCommand { get; set; }
        public ReactiveCommand<eBook, Unit> UpdateeBookCommand { get; set; }
        public ReactiveCommand<int, Unit> RemoveBookCommand { get; set; }
        public ReactiveCommand<int, Unit> RemoveeBookCommand { get; set; }
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
            UpdateBookCommand = ReactiveCommand.CreateFromTask((Book book) => UpdateBookCommandMethod(book));
            UpdateeBookCommand = ReactiveCommand.CreateFromTask((eBook ebook) => UpdateeBookCommandMethod(ebook));
            RemoveBookCommand = ReactiveCommand.CreateFromTask((int id) => RemoveBookCommandMethod(id));
            RemoveeBookCommand = ReactiveCommand.CreateFromTask((int id) => RemoveeBookCommandMethod(id));
            AddeBookCommand = ReactiveCommand.CreateFromTask(() => AddeBookCommandMethod());
            AddEventCommand = ReactiveCommand.CreateFromTask(() => AddEventCommandMethod());
            ToggleHidden = ReactiveCommand.CreateFromTask((object param) => HiddenCommandMethod(param));
            ToggleVisible = ReactiveCommand.CreateFromTask((object param) => VisibleCommandMethod(param));
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

        /// <summary>
        /// Updates a Book in DB
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public async Task UpdateBookCommandMethod(Book book)
        #region ...
        {
            await bookRepo.Update(book);
            await LoadBooks();
        }
        #endregion

        public async Task UpdateeBookCommandMethod(eBook ebook)
        #region ...
        {
            await eBookRepo.Update(ebook);
            await LoadBooks();
        }
        #endregion

        /// <summary>
        /// Removes Book From DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        public async Task RemoveeBookCommandMethod(int id)
        {
            if(ReasonToDelete == null)
            {
                MessageBox.Show("Fyll i anledning!");
                return;
            }
            await eBookRepo.Delete(id);
            await LoadeBooks();
        }
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
            await LoadeBooks();
            await ClearBookLines("ebooks");
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
        /// Loads all the data from DB
        /// </summary>
        public async void LoadDataAsync()
        #region ...
        {
            await LoadBooks();
            await LoadeBooks();
            await LoadEvents();
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
            foreach (var book in await bookRepo.ReadAll())
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
            foreach (var ebook in await eBookRepo.ReadAll())
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

    }
}
