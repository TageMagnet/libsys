using LIBSYS.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Library;

namespace LIBSYS.ViewModels
{
    public class LoginViewModel: ViewModelBase
    {
        public MemberRepository repo = new MemberRepository();
        public List<Member> memberList;
        public string username { get; set; }
        public string password { get; set; }

        public ReactiveCommand<Unit, Unit> LoginCommand { get; set; }
        public LoginViewModel()
        {
            LoadDataAsync();
            LoginCommand = ReactiveCommand.Create(() => LoginMethod());
        }

        //Skapa loginmetod
        public async void LoadDataAsync()
        {
            await Task.Run(() => GetMembers());
        }

        public async Task GetMembers()
        {
            memberList = new List<Member>(await repo.ReadAll());
        }
        public void LoginMethod()
        {

            // Metoden ska ta in användarnamn / lösen  från LoginView. kontrollera? samt returnera den användarens roll :)

            //Använda denna för att sätta in en ny user. Men ändra på namn m.m eftersom min redan finns
            var newMember = new Member { email = "pontus.pettsson@hotmail.com", nickname = "inte_golden", pwd = "1234", role = "librarian" };
            repo.Create(newMember);

            //Använd denna för att skapa seminarium
            //int id = repo.CreateEvent("Seminarie", new DateTime(2020, 04, 05), new DateTime(2020, 05, 06), "StatsBiblioteket", 3);

            
        }

    }

}
