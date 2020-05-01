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
        //public MemberRepository repo = new MemberRepository();
        public List<Member> memberList;
        public string username { get; set; }
        public string password { get; set; }

        public ReactiveCommand<Unit, Unit> LoginCommand { get; set; }
        public ReactiveCommand<Unit, Unit> GoBackCommand { get; set; }
        public LoginViewModel()
        {
            MainWindowViewModel.ChangeView("librarian");
            //LoadDataAsync();
            LoginCommand = ReactiveCommand.Create(() => LoginMethod());
            GoBackCommand = ReactiveCommand.Create(() => MainWindowViewModel.ChangeView("home"));
            memberList = new List<Member>();
            memberList.Add(new Member { nickname = "Jonathan", email = "jh@email.com", pwd = "jh123", role = "admin" });
            memberList.Add(new Member { nickname = "Pontus", email = "pp@email.com", pwd = "pp123", role = "librarian" });
            memberList.Add(new Member { nickname = "Thomas", email = "tc@email.com", pwd = "tc123", role = "customer" });
        }

        //Skapa loginmetod
        public async void LoadDataAsync()
        {
            await Task.Run(() => GetMembers());
        }

        public async Task GetMembers()
        {
            //memberList = new List<Member>(await repo.ReadAll());
        }
        public void LoginMethod()
        {
            var member = new Member();


            if (String.IsNullOrWhiteSpace(username))
                return;

            if (String.IsNullOrWhiteSpace(password))
                return;

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
