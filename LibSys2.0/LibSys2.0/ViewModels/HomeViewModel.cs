using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Library;
using MessageBox = System.Windows.MessageBox;
using System.Collections.Specialized;
using System.Windows.Data;
using System.Linq;
using LibrarySystem.Views;
using LibrarySystem.ViewModels.Backend;

namespace LibrarySystem.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        #region Properties
        private ItemRepository itemRepository = new ItemRepository();

        /// <summary>
        /// Used for data-triggers
        /// </summary>
        public string State { get; set; } = "initial";

        /// <summary>Animation after search action</summary>
        public RelayCommandWithParameters TestCommand { get; set; }

        /// <summary>Search for book using string from search-field</summary>
        public RelayCommandWithParameters SearchCommand { get; set; }

        /// <summary>Which database column to search in</summary>
        public RelayCommandWithParameters SetSearchColumn { get; set; }

        /// <summary>Paste autocomplete click into searchbox</summary>
        public RelayCommandWithParameters PasteToSearchBox { get; set; }

        /// <summary>Whenever loan button is hit by user</summary>
        public RelayCommandWithParameters LoanBookCommand { get; set; }

        /// <summary>Pagination decrement</summary>
        public RelayCommand PreviousPage { get; set; }

        /// <summary>Pagination increment</summary>
        public RelayCommand NextPage { get; set; }

        /// <summary>
        /// Starts up a browser and visits download link
        /// </summary>
        public RelayCommandWithParameters GoToBrowserLink { get; set; }
        public RelayCommand GotoInfoCommand { get; set; }

        /// <summary>
        /// Returnt results
        /// </summary>
        public ObservableCollection<SearchItem> SearchResults { get; set; } = new ObservableCollection<SearchItem>();

        /// <summary>
        /// Used as trigger to hide counters
        /// </summary>
        public bool ShowSearchResults { get; set; } = false;

        /// <summary>
        /// Helper proxxy for filtering and paging search results
        /// </summary>
        public CollectionViewSource PaginationList { get; set; } = new CollectionViewSource();

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
        /// Active page
        /// </summary>
        public int CurrentSearchPage { get; set; } = 1;

        private int resultsPerPage = 5;
        /// <summary>
        /// Rows <see cref="SearchResults"></see> per page
        /// </summary>
        public int ResultsPerPage
        {
            get => resultsPerPage;
            set
            {
                resultsPerPage = value;
                PaginationList.View.Refresh();
            }
        }

        /// <summary>Row per page counter MIN</summary>
        public int CurrentPageMin { get => (ResultsPerPage * CurrentSearchPage) - ResultsPerPage + 1; }

        /// <summary>Row per page counter MAX</summary>
        public int CurrentPageMax
        {
            get
            {
                // Safe return if triggered before search has been done
                if (SearchResultCount < 1)
                    return 0;
                // Special remainder logic for the last page
                // e.g. total is 9 results, 5 first page and 4 last page
                int a = (ResultsPerPage * CurrentSearchPage);
                int b = a % SearchResultCount;
                return b < a ? a - b : a;
            }
        }

        /// <summary>
        /// ...
        /// </summary>
        public List<int> ResultPerPageOptions { get; set; } = new List<int>() { 5, 10, 15 };

        // Private holder
        private double resultsDividedPerPage { get; set; }
        /// <summary>
        /// Logic for pagination, total count divided by items per page.
        /// </summary>
        public double ResultsDividedPerPage
        {
            get => Math.Ceiling(Convert.ToDouble(SearchResultCount) / Convert.ToDouble(ResultsPerPage));
            set { resultsDividedPerPage = value; }
        }

        // Private holder
        private string searchFieldText { get; set; }
        /// <summary>
        /// Text for the SearchFieldInput textbox
        /// </summary>
        public string SearchFieldText
        {
            get => searchFieldText;
            set
            {
                searchFieldText = value;

                // Clear old autocomplete
                AutoCompleteList.Clear();

                // Break if fewer than two letters are typed in
                if (searchFieldText.Length >= 2)
                    LoadAutoCompleteResults();

                // Notify about change
                OnPropertyChanged(nameof(SearchFieldText));
            }
        }

        /// <summary>
        /// ...
        /// </summary>
        public int ColumnSearchBar { get; set; } = 1;

        /// <summary>
        /// ...
        /// </summary>
        public int RowSearchBar { get; set; } = 2;

        /// <summary>
        /// Sets when x:Name SearchField is filled in, limited to 2 for now
        /// </summary>
        public ObservableCollection<string> AutoCompleteList { get; set; } = new ObservableCollection<string>();

        #endregion

        /// <summary>
        /// Construct on ViewModel load
        /// </summary>
        public HomeViewModel()
        {
            // Note. Paginationlist is acting as a proxy for SearchResults
            // While SearchResults contains the actual data, PaginationList is like a facet controlling the output
            PaginationList.Source = SearchResults;
            PaginationList.Filter += new FilterEventHandler(ViewFilter);

            // Previous page on the pagination
            PreviousPage = new RelayCommand(() =>
            {
                // Check to avoid going negative
                CurrentSearchPage -= CurrentSearchPage > 1 ? 1 : 0;
                PaginationList.View.Refresh();
                NotifyPropertyChanged(nameof(CurrentSearchPage));
                NotifyPropertyChanged(nameof(CurrentPageMin));
                NotifyPropertyChanged(nameof(CurrentPageMax));
            });
            // Next page on the pagination
            NextPage = new RelayCommand(() =>
            {
                // Disabled at last pagination page
                CurrentSearchPage += CurrentSearchPage < ResultsDividedPerPage ? 1 : 0;
                PaginationList.View.Refresh();
                NotifyPropertyChanged(nameof(CurrentSearchPage));
                NotifyPropertyChanged(nameof(CurrentPageMin));
                NotifyPropertyChanged(nameof(CurrentPageMax));
            });

            // Using the default browser, go to specified adress
            GoToBrowserLink = new RelayCommandWithParameters((param) =>
            {
                // Logged in variable required
                if (!Globals.IsLoggedIn)
                {
                    MessageBox.Show("Du behöver först logga in eller skapa ett konto för att låna böcker");
                    return;
                }
                // Card blockage check
                if (Globals.LoggedInUser.cardstatus != 1)
                {
                    MessageBox.Show("Ditt kort är spärrat. Kontakta en bibliotikarie");
                    return;
                }
                // Validate url
                if (!Etc.Utilities.UrlChecker(param.ToString()))
                {
                    MessageBox.Show("Invalid URL för nedladdning, kontakta adminstratör");
                    return;
                }

                // Like starting a program through command shell
                var psi = new System.Diagnostics.ProcessStartInfo() { FileName = param.ToString(), UseShellExecute = true };
                System.Diagnostics.Process.Start(psi);
            });

            //todo; not in use. remove
            SetSearchColumn = new RelayCommandWithParameters((param) =>
            {
                SearchColumn = (string)param;
            });

            SearchCommand = new RelayCommandWithParameters(async (param) =>
            {
                await SearchCommandAction((string)param);
                TryRunAnimation();
            });

            PasteToSearchBox = new RelayCommandWithParameters(async (param) =>
            {
                // Paste value
                SearchFieldText = (string)param;
                // Clear autocomplete suggestion box
                AutoCompleteList.Clear();
                await SearchCommandAction(SearchFieldText);
                TryRunAnimation();
            });

            LoanBookCommand = new RelayCommandWithParameters(async (param) => await LoanBook((SearchItem)param));

            GotoInfoCommand = new RelayCommand(async () => await GotoInfoMethod());

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
                return;
            }
            if (Globals.LoggedInUser.cardstatus != 1)
            {
                MessageBox.Show("Ditt kort är spärrat. Kontakta en bibliotikarie");
                return;
            }

            Member currentMember = Globals.LoggedInUser;

            // Convert SearchItem into Item
            Item item = new Item(searchItem);

            //Uppdatera GUI
            searchItem.Available--;
            searchItem.UnAvailable++;

            // Check if there are enough in stock
            if (searchItem.Available < 0)
            {
                MessageBox.Show("Slut på bok!");
                return;
            }

            await itemRepository.SubscribeToItem(item, currentMember);
            SearchResults.Clear();
            await LoadSearchResults(SearchFieldText);

            MessageBox.Show("Bok lånad!");
        }

        /// <summary>
        /// -//- Overloading to <see cref="LoanBook(SearchItem)"></see>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private async Task LoanBook(Item item) => await LoanBook(new SearchItem(item));

        /// <summary>
        /// Search action calling on SQL repo
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private async Task SearchCommandAction(string arg)
        {
            // Clear old search
            SearchResults.Clear();
            CurrentSearchPage = 1;

            // Load new
            await LoadSearchResults(arg);
            AutoCompleteList.Clear();
        }

        /// <summary>
        /// Items that are seen in the 'autocomplete' box after typing in at least $n characters
        /// </summary>
        private async void LoadAutoCompleteResults()
        {
            AutoCompleteList.Clear();
            // Load repos
            var items = await itemRepository.SearchQueryWithStatuses(SearchFieldText);

            List<string> isbnCodes = new List<string>();
            List<SearchItem> SortedItems = new List<SearchItem>();
            foreach (SearchItem item in items)
            {
                string isbn = item.isbn;

                if (isbnCodes.Contains(isbn))
                {
                    continue;
                }

                isbnCodes.Add(item.isbn);
                SortedItems.Add(item);
            }
            // The first 3
            int j = 0;
            int max = 3;
            foreach (SearchItem item in SortedItems)
            {
                if (j >= max)
                    break;
                AutoCompleteList.Add(item.title);
                j++;
            }
        }

        /// <summary>
        /// After hitting search button, insert data into observable collections and notifies about changes via propertychanged
        /// </summary>
        private async Task LoadSearchResults(string arg)
        {
            // For use in Item.Index method
            int i = 0;

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

                //Increment index for later use
                item.Index = i;
                i += 1;

                //Convert into an SearchItem, since we need the additional DuplicateCounter property
                //SearchItem searchItem = new SearchItem(item);
                SearchResults.Add(item);
            }

            if (SearchResults.Count > 0)
            {
                ShowSearchResults = true;
            }

            // Notify the counters
            NotifyPropertyChanged(nameof(ShowSearchResults));
            NotifyPropertyChanged(nameof(SearchResultCount));
            NotifyPropertyChanged(nameof(ResultsDividedPerPage));
            NotifyPropertyChanged(nameof(CurrentPageMin));
            NotifyPropertyChanged(nameof(CurrentPageMax));
        }

        /// <summary>
        /// To allows for pagination. Max allowed results per page == <see cref="ResultsPerPage"></see>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ViewFilter(object sender, FilterEventArgs e)
        {
            SearchItem item = (SearchItem)e.Item;
            int max = CurrentSearchPage * ResultsPerPage;
            int min = (CurrentSearchPage * ResultsPerPage) - ResultsPerPage;

            if (min <= item.Index && item.Index < max)
            {
                e.Accepted = true;
            }
            else
            {
                e.Accepted = false;
            }
        }

        /// <summary>
        /// Method to open up Site Info-window
        /// </summary>
        /// <returns></returns>
        private async Task GotoInfoMethod()
        {
            var infoView = new SiteInfoView();
            infoView.DataContext = new SiteInfoViewModel();
            infoView.ShowDialog();
        }

        private async void TryRunAnimation()
        {
            // Break if already searched once, searchpanel is at top position
            if (State == "search")
                return;
            // Allow animation to catch up
            Task delay = Task.Delay(300);
            await delay;
            // Jump element one row up 
            if (RowSearchBar != 1)
                RowSearchBar = 1;
            State = "search";
        }
    }
}
