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
    public class LoginViewModel : ViewModelBase
    {
        public MemberRepository memberRepo = new MemberRepository();
        public List<Member> memberList;
        public string username { get; set; }
        public string password { get; set; }

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


            if (String.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Fyll i användarnamn");
                return;
            }

            if (String.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Fyll i lösenord");
                return;
            }

            var user = memberList.Find(x => x.email == username);

            if (user == null)
            {
                MessageBox.Show("Fel Lösenord eller användarnamn");
                return;
            }

            if (user.pwd != password)
            {
                MessageBox.Show("Fel Lösenord eller användarnamn");
                return;
            }
            MainWindowViewModel.ChangeView(user.role);
        }

    }
}
