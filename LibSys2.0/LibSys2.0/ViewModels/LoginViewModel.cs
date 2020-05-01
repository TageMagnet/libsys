using Library;
using LibrarySystem.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LibrarySystem.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public MemberRepository memberRepo = new MemberRepository();
        public List<Member> memberList;
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

        public ReactiveCommand<Unit, Unit> LoginCommand { get; set; }
        public ReactiveCommand<Unit, Unit> GoBackCommand { get; set; }
        public LoginViewModel()
        {
            LoadDataAsync();

            LoginCommand = ReactiveCommand.Create(() => LoginMethod());
            GoBackCommand = ReactiveCommand.Create(() => MainWindowViewModel.ChangeView("home"));
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
        public void LoginMethod()
        {
            var member = new Member();


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

            var user = memberList.Find(x => x.email == Username);

            if (user == null)
            {
                MessageBox.Show("Fel Lösenord eller användarnamn");
                Password = "";
                return;
            }

            if (user.pwd != Password)
            {
                MessageBox.Show("Fel Lösenord eller användarnamn");
                Password = "";
                return;
            }
            MainWindowViewModel.ChangeView(user.role);
        }

    }
}
