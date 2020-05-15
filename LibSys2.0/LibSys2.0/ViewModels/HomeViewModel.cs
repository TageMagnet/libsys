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
        /// <summary>
        /// Antal <see cref="SearchResults"></see> per page
        /// </summary>
        public int ResultsPerPage { get; set; } = 5;
        // Private holder
        private double resultsDividedPerPage { get; set; }
        public double ResultsDividedPerPage
        {
            get => Math.Ceiling(Convert.ToDouble(SearchResultCount) / Convert.ToDouble(ResultsPerPage));
            set { resultsDividedPerPage = value; }
        }

        public ViewModels.Components.SearchPageControl SearchPageControl { get; set; } = new ViewModels.Components.SearchPageControl();
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

        public HomeViewModel()
        {
            SearchCommand = ReactiveCommand.Create((string value) => SearchCommandAction(value));
            SetSearchColumn = ReactiveCommand.Create((string value) =>
            {
                SearchColumn = value;
            });
            PasteToSearchBox = ReactiveCommand.Create((string value) =>
            {
                SearchFieldText = value;
                SearchCommandAction(SearchFieldText);
                AutoCompleteList.Clear();
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
            var books = await repo.SearchQuery(SearchFieldText);
            var eBooks = await repo2.SearchQuery(SearchFieldText);
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
            var books = await repo.SearchQuery(arg);
            var eBooks = await repo2.SearchQuery(arg);

            // Loop and add them into the view
            foreach (IArticle book in books)
            {
                book.IsEbook = false;
                SearchResults.Add(book);
            }
            foreach (eBook ebook in eBooks)
            {
                ebook.IsEbook = true;
                SearchResults.Add(ebook);
            }

            // Notify the counters
            NotifyPropertyChanged("SearchResultCount");
            NotifyPropertyChanged("ResultsDividedPerPage");
        }
    }
}
