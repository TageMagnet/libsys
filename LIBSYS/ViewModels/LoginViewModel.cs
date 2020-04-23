using LIBSYS.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using System.Linq;

namespace LIBSYS.ViewModels
{
    public class LoginViewModel: ViewModelBase
    {
        public Repository repo = new Repository();
        public List<Member> memberList;
        public string username { get; set; }
        public string password { get; set; }

        public ReactiveCommand<Unit, Unit> LoginCommand { get; set; }
        public LoginViewModel()
        {
            //memberList = new List<Member>();
            //memberList.Add(new Member { nickName = "janne", pwd = "123", role = "admin" });

            memberList = new List<Member>(repo.ReadMembers());
            LoginCommand = ReactiveCommand.Create(() => LoginMethod());
        }

        //Skapa loginmetod

        public void LoginMethod()
        {
            //Använda denna för att sätta in en ny user. Men ändra på namn m.m eftersom min redan finns
            //repo.CreateMember("Jonathan.harlin@hotmail.com", "Golden", "123", "Admin");
            
        }

    }

}
