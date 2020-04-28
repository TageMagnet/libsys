using LIBSYS.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Library;
using Avalonia.Controls;

namespace LIBSYS.ViewModels
{
    public class LoginViewModel: ViewModelBase
    {
        //public MemberRepository repo = new MemberRepository();
        public List<Member> memberList;
        public string username { get; set; }
        public string password { get; set; }

        public ReactiveCommand<Unit, Unit> LoginCommand { get; set; }
        public LoginViewModel()
        {
            //LoadDataAsync();
            LoginCommand = ReactiveCommand.Create(() => LoginMethod());
            memberList = new List<Member>();
            memberList.Add(new Member { nickname = "Jonathan", email = "jh@email.com", pwd = "jh123", role="admin" });
            memberList.Add(new Member { nickname = "Pontus", email = "pp@email.com", pwd = "pp123", role="librarian" });
            memberList.Add(new Member { nickname = "Thomas", email = "tc@email.com", pwd = "tc123", role="customer" });
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
            if (!String.IsNullOrWhiteSpace(username) || !String.IsNullOrWhiteSpace(password))
            {
                var user = memberList.Find(x => x.email == username);
                if (user.pwd == password)
                {
                    MainWindowViewModel.ChangeView(user.role);
                }
                
                //ShowMessageBox()
                
            }
        }

    }

}
