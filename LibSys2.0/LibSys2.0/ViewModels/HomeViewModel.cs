using LibrarySystem.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        /// <summary>
        /// Kopplad till "Login"-<see cref="System.Windows.Controls.Button"/>
        /// </summary>
        public ReactiveCommand<Unit, Unit> LoginCommand { get; set; }
        /// <summary>
        /// Register -//-
        /// </summary>
        public ReactiveCommand<Unit, Unit> RegisterCommand { get; set; }
        /// <summary>
        /// Demo för propertychanged, kan tas bort
        /// </summary>
        public ReactiveCommand<Unit, Unit> TestChange { get; set; }

        /// <summary>
        /// Search for book using string from search-field
        /// </summary>
        public ReactiveCommand<string, Unit> SearchCommand { get; set; }

        /// <summary>
        /// Returned results from SearchCommand
        /// </summary>
        public ObservableCollection<Book> BookSearchResults { get; set; } = new ObservableCollection<Book>();
        /// <summary>
        /// -//-
        /// </summary>
        public ObservableCollection<eBook> eBookSearchResults { get; set; } = new ObservableCollection<eBook>();
        /// <summary>
        /// Simple counter return for list
        /// </summary>
        public int eBookSearchResultCount{get => eBookSearchResults.Count;}
        /// <summary>
        /// -//-
        /// </summary>
        public int BookSearchResultCount { get => BookSearchResults.Count; }

        public string Text { get; set; } = "Hello world";

        public HomeViewModel()
        {
            LoginCommand = ReactiveCommand.Create(() => MainWindowViewModel.ChangeView("librarian"));
            RegisterCommand = ReactiveCommand.Create(() => MainWindowViewModel.ChangeView("register"));
            TestChange = ReactiveCommand.Create(() => {
                Text = "I WAS UPDATED";
                OnPropertyChanged("Text");
            });
            SearchCommand = ReactiveCommand.Create((string value) => SearchCommandAction(value));

        }

        private void SearchCommandAction(string arg)
        {
            // Clear old search
            BookSearchResults.Clear();
            eBookSearchResults.Clear();

            LoadSearchResults(arg);
        }

        private async void LoadSearchResults(string arg)
        {
            // Load repos
            var repo = new Library.BookRepository();
            var repo2 = new Library.eBookRepository();
            // Do the search queries
            var books = await repo.SearchByTitle(arg);
            var eBooks = await repo2.SearchByTitle(arg);

            // Loop and add them into the view
            foreach (Book book in books)
            {
                BookSearchResults.Add(book);
            }
            foreach (eBook ebook in eBooks)
            {
                eBookSearchResults.Add(ebook);
            }

            // Notify the counters
            NotifyPropertyChanged("BookSearchResultCount");
            NotifyPropertyChanged("eBookSearchResultCount");
        }
    }
}
