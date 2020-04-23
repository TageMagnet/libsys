using LIBSYS.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;

namespace LIBSYS.ViewModels
{
    public class LoginViewModel: ViewModelBase
    {
        //public Repository repo = new Repository();
        public List<Member> memberList;
        public string username { get; set; }
        public string password { get; set; }

        public ReactiveCommand<Unit, Unit> LoginCommand { get; set; }
        public LoginViewModel()
        {
            memberList = new List<Member>();
            memberList.Add(new Member { nickName = "janne", pwd = "123", role = "admin" });

            LoginCommand = ReactiveCommand.Create(() => LoginMethod());
        }

        //Skapa loginmetod

        public void LoginMethod()
        {
            // Metoden ska ta in användarnamn / lösen  från LoginView. kontrollera? samt returnera den användarens roll :)
            
        }

    }

}
