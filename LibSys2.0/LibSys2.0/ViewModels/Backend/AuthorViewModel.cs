using Library;
using LibrarySystem.Models;
using LibrarySystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using MessageBox = System.Windows.MessageBox;

namespace LibrarySystem
{
    public class AuthorViewModel : BaseViewModel
    {
        private ItemRepository itemRepo = new ItemRepository();
        private AuthorRepository authorRepo = new AuthorRepository();
        public ObservableCollection<Author> Authors { get; set; } = new ObservableCollection<Author>();

        public Author SelectedAuthor { get; set; } = new Author();
        public RelayCommand AddAuthorCommand { get; set; }
        public RelayCommandWithParameters UpdateAuthorCommand { get; set; }
        public RelayCommandWithParameters RemoveAuthorCommand { get; set; }
        

        public AuthorViewModel()
        {
            AddAuthorCommand = new RelayCommand(async () => await AddAuthorCommandMethod());
            UpdateAuthorCommand = new RelayCommandWithParameters(async (param) => await UpdateAuthorCommandMethod((Author)param));
            RemoveAuthorCommand = new RelayCommandWithParameters(async (param) => await RemoveAuthorCommandMethod((int)param));
            LoadDataAsync();
        }

        /// <summary> Method to add author to DB </summary>
        public async Task AddAuthorCommandMethod()
        {
            if (SelectedAuthor.firstname == null)
            {
                MessageBox.Show("Lägg till Förnamn!");
                return;
            }
            if (SelectedAuthor.surname == null)
            {
                MessageBox.Show("Lägg till efternamn!");
                return;
            }
            if (SelectedAuthor.nickname == null)
            {
                MessageBox.Show("Lägg till smeknamn!");
                return;
            }

            await authorRepo.Create(SelectedAuthor);
            await LoadAuthors();
            await ClearAuthorLines();
        }

        /// <summary> Updates a author to db.</summary>
        /// <param name="author"></param>
        public async Task UpdateAuthorCommandMethod(Author author)
        {
            await authorRepo.Update(author);
            await LoadAuthors();
        }

        /// <summary>Removes/delete a author from DB</summary>
        /// <param name="id"></param>
        public async Task RemoveAuthorCommandMethod(int id)
        {
            int numberOfAuthorBooks = 0;

            foreach (var item in await itemRepo.ReadAll())
            {
                if (item.ref_author_id == id)
                    numberOfAuthorBooks++;
            }

            if (numberOfAuthorBooks > 0)
            {
                MessageBox.Show($"Denna författare är bunden till {numberOfAuthorBooks}st böcker. Och kan därför inte tas bort");
                await LoadAuthors();
                return;
            }
            try
            {
                await authorRepo.Delete(id);
            }
            catch (Exception)
            {
                MessageBox.Show("Författaren är bunden till en bok. Går ej att ta bort.");
            }
            finally
            {
                await LoadAuthors();
            }

        }

        public async Task ClearAuthorLines()
        {
            SelectedAuthor.firstname = "";
            SelectedAuthor.surname = "";
            SelectedAuthor.nickname = "";
            OnPropertyChanged(nameof(SelectedAuthor));
        }

        /// <summary>Loads all the data from DB</summary>
        public async void LoadDataAsync()
        {
            await LoadAuthors();
        }

        /// <summary>Reloads all the Authors from DB</summary>
        public async Task LoadAuthors()
        {
            Authors.Clear();
            foreach (var author in await authorRepo.ReadAll())
            {
                Authors.Add(author);
            }
        }
    }
}
