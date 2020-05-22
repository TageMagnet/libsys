using Library;
using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;
using Ubiety.Dns.Core;

namespace LibrarySystem.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public MemberRepository memberRepo = new MemberRepository();
        public List<Member> memberList;
        HeaderViewModel headerViewModel = new HeaderViewModel();
        public string Username { get; set; }


        // Backing field
        private string password;
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }

        public RelayCommand LoginCommand { get; set; }
        public RelayCommand GoBackCommand { get; set; }

        public RelayCommand GoToRegister { get; set; }
        public LoginViewModel()
        {
            LoadDataAsync();

            LoginCommand = new RelayCommand(async () => await LoginMethod());
            GoBackCommand = new RelayCommand(() => MainWindowViewModel.ChangeView("home"));
            GoToRegister = new RelayCommand(() => MainWindowViewModel.ChangeView("register"));
        }

        //Skapa loginmetod
        public async void LoadDataAsync()
        {
            await Task.Run(() => GetMembers());
        }

        public async Task GetMembers()
        {
            memberList = new List<Member>(await memberRepo.ReadAll());
        }
        public async Task LoginMethod()
        {
            Member user = new Member();


            if (String.IsNullOrWhiteSpace(Username))
            {
                MessageBox.Show("Fyll i användarnamn");
                return;
            }

            if (String.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Fyll i lösenord");
                return;
            }
            Username = Username.ToLower();

            user = (await memberRepo.SearchByColumn("email", Username)).Find(x => x.email == Username); //.Find(x => x.email == Username);
            

            // Null check if user does not exist in database
            if (user == null)
            {
                MessageBox.Show("Fel Lösenord eller användarnamn");
                Password = "";
                return;
            }

            if (user.is_active < 1)
            {
                MessageBox.Show("Ditt konto är inaktiverat");
                return;
            }


            if (user.pwd != Password)
            {
                MessageBox.Show("Fel Lösenord eller användarnamn");
                Password = "";
                return;
            }

            await Task.Run(async() => {
                // Do something with loading bar here
                await Task.Delay(1);
            });

            //await headerViewModel.ShowLoggedIn();
            MainWindowViewModel.ChangeView("home");
            Globals.LoggedInUser = user;
            OnPropertyChanged("CurrentLoggedInMember");
            
        }

    }
}
