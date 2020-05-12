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
        /// Which database column to search in
        /// </summary>
        public ReactiveCommand<string, Unit> SetSearchColumn { get; set; }
        /// <summary>
        /// Paste autocomplete click into searchbox
        /// </summary>
        public ReactiveCommand<string, Unit> PasteToSearchBox { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<IArticle> SearchResults { get; set; } = new ObservableCollection<IArticle>();
        // Defaulted to 'title'
        private string searchColumn { get; set; } = "title";
        /// <summary>
        /// Todo; do something better
        /// </summary>
        public string SearchColumn { get => searchColumn; set { searchColumn = value; OnPropertyChanged("SearchColumn"); } }
        /// <summary>
        /// Simple counter return for list
        /// </summary>
        public int SearchResultCount { get => SearchResults.Count; }
        // Private holder
        private string searchFieldText { get; set; }
        /// <summary>
        /// x:Name SearchField Text
        /// </summary>
        public string SearchFieldText
        {
            get => searchFieldText;
            set
            {
                searchFieldText = value;
                // Make autocomplete query if letters are more or equal to number
                AutoCompleteList.Clear();
                if (searchFieldText.Length >= 2)
                {
                    LoadAutoCompleteResults();
                }
                OnPropertyChanged("SearchFieldText");
            }
        }
        /// <summary>
        /// Sets when x:Name SearchField is filled in, limited to 3 for now
        /// </summary>
        public ObservableCollection<string> AutoCompleteList { get; set; } = new ObservableCollection<string>();

        public string Text { get; set; } = "Hello world";

        public HomeViewModel()
        {
            LoginCommand = ReactiveCommand.Create(() => MainWindowViewModel.ChangeView("librarian"));
            RegisterCommand = ReactiveCommand.Create(() => MainWindowViewModel.ChangeView("register"));
            TestChange = ReactiveCommand.Create(() =>
            {
                Text = "I WAS UPDATED";
                OnPropertyChanged("Text");
            });
            SearchCommand = ReactiveCommand.Create((string value) => SearchCommandAction(value));
            SetSearchColumn = ReactiveCommand.Create((string value) =>
            {
                SearchColumn = value;
            });
            PasteToSearchBox = ReactiveCommand.Create((string value) =>
            {
                SearchFieldText = value;
                SearchCommandAction(SearchFieldText);
                //this.OnPropertyChanged(nameof(value));
            });
        }

        private void SearchCommandAction(string arg)
        {
            // Clear old search
            SearchResults.Clear();
            // Load new
            LoadSearchResults(arg);
        }

        private async void LoadAutoCompleteResults()
        {
            AutoCompleteList.Clear();
            // Load repos
            var repo = new Library.BookRepository();
            var repo2 = new Library.eBookRepository();
            // Do the search queries
            var books = await repo.SearchByColumn(SearchColumn, SearchFieldText, 3);
            var eBooks = await repo2.SearchByColumn(SearchColumn, SearchFieldText, 3);
            // The first 3
            int j = 0;
            int max = 3;
            foreach (Book book in books)
            {
                if (j >= max)
                    break;
                AutoCompleteList.Add(book.title);
                j++;
            }
            foreach (eBook ebook in eBooks)
            {
                if (j >= max)
                    break;
                AutoCompleteList.Add(ebook.title);
                j++;
            }

            // Notify autocomplete
            //NotifyPropertyChanged("AutoCompleteList");
        }

        private async void LoadSearchResults(string arg)
        {
            // Load repos
            var repo = new Library.BookRepository();
            var repo2 = new Library.eBookRepository();
            // Do the search queries
            var books = await repo.SearchByColumn(SearchColumn, arg);
            var eBooks = await repo2.SearchByColumn(SearchColumn, arg);

            // Loop and add them into the view
            foreach (Book book in books)
            {
                SearchResults.Add(book);
            }
            foreach (eBook ebook in eBooks)
            {
                SearchResults.Add(ebook);
            }

            // Notify the counters
            NotifyPropertyChanged("SearchResultCount");
        }
    }
}
