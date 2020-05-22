using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Library;
using MessageBox = System.Windows.MessageBox;

namespace LibrarySystem.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {

        private ItemRepository itemRepository = new ItemRepository();
        /// <summary>Search for book using string from search-field</summary>
        public RelayCommandWithParameters SearchCommand { get; set; }
        /// <summary>Which database column to search in</summary>
        public RelayCommandWithParameters SetSearchColumn { get; set; }
        /// <summary>Paste autocomplete click into searchbox</summary>
        public RelayCommandWithParameters PasteToSearchBox { get; set; }
        public RelayCommandWithParameters LoanBookCommand { get; set; }

        /// <summary>
        /// Returnt results
        /// </summary>
        public ObservableCollection<SearchItem> SearchResults { get; set; } = new ObservableCollection<SearchItem>();
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
        public int CurrentSearchPage { get; set; } = 0;

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
        /// Sets when x:Name SearchField is filled in, limited to 2 for now
        /// </summary>
        public ObservableCollection<string> AutoCompleteList { get; set; } = new ObservableCollection<string>();

        public HomeViewModel()
        {
            // Init empty user
            SearchCommand = new RelayCommandWithParameters(async (param) => await SearchCommandAction((string)param));
            SetSearchColumn = new RelayCommandWithParameters((param) =>
            {
                SearchColumn = (string)param;
            });
            PasteToSearchBox = new RelayCommandWithParameters(async (param) =>
            {
                SearchFieldText = (string)param;
                await SearchCommandAction(SearchFieldText);
                AutoCompleteList.Clear();
            });
            LoanBookCommand = new RelayCommandWithParameters(async (param) => await LoanBook((SearchItem)param));
        }


        /// <summary>
        /// Loan book action, adds the item to the logged in users subscriptions
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private async Task LoanBook(SearchItem searchItem)
        {
            if (!Globals.IsLoggedIn)
            {
                MessageBox.Show("Du behöver först logga in eller skapa ett konto för att låna böcker");
                //MainWindowViewModel.ChangeView("login");
                return;
            }

            Member currentMember = Globals.LoggedInUser;

            // Convert SearchItem into Item
            Item item = new Item(searchItem);

            await itemRepository.SubscribeToItem(item, currentMember);
            MessageBox.Show("Bok lånad!");
        }

        /// <summary>
        /// -//- Overloading to <see cref="LoanBook(SearchItem)"></see>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private async Task LoanBook(Item item) => await LoanBook(new SearchItem(item));

        /// ...
        private async Task SearchCommandAction(string arg)
        {
            // Clear old search
            SearchResults.Clear();
            // Load new
            await LoadSearchResults(arg);
        }

        /// <summary>
        /// Items that are seen in the 'autocomplete' box after typing in at least $n characters
        /// </summary>
        private async void LoadAutoCompleteResults()
        {
            AutoCompleteList.Clear();
            // Load repos
            var items = await itemRepository.SearchQuery(SearchFieldText);

            // The first 3
            int j = 0;
            int max = 3;
            foreach (Item item in items)
            {
                if (j >= max)
                    break;
                AutoCompleteList.Add(item.title);
                j++;
            }
        }

        /// <summary>
        /// After hitting search button, loads observable collections with data and notifies the XAML
        /// </summary>
        private async Task LoadSearchResults(string arg)
        {
            // Load repos
            var items = await itemRepository.SearchQueryWithStatuses(SearchFieldText);

            // Keep track of ISBN to check for duplicates
            List<string> isbnCodes = new List<string>();

            // Loop and add them into the view
            foreach (SearchItem item in items)
            {
                // Hold the string to avoid code duplication
                string isbn = item.isbn;

                if (isbnCodes.Contains(isbn))
                {
                    // If duplicate, increment the counter
                    int index = isbnCodes.FindIndex(x => x == isbn);
                    //SearchResults[index].Total++;

                    // Skip to next element
                    continue;
                }

                // Add to duplicate-checker list
                isbnCodes.Add(item.isbn);

                //Convert into an SearchItem, since we need the additional DuplicateCounter property
                //SearchItem searchItem = new SearchItem(item);
                SearchResults.Add(item);
            }

            // Notify the counters
            NotifyPropertyChanged("SearchResultCount");
            NotifyPropertyChanged("ResultsDividedPerPage");
        }
    }
}
