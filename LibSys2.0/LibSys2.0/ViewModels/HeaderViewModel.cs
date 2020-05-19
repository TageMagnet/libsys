using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LibrarySystem.ViewModels
{
    public class HeaderViewModel : BaseViewModel
    {
        public RelayCommandWithParameters GoHome { get; set; }
        public RelayCommand GoToLogin { get; set; }
        public RelayCommand GoToRegister { get; set; }
        public RelayCommand ByPassIntoBackend { get; set; }
        //
        /// <summary> Extends the currently logged in member from MainWindow </summary>
        //public Member CurrentLoggedInMember { get => Globals.LoggedInUser; set => Globals.LoggedInUser = value; }
        //public bool IsLoggedIn
        //{
        //    get => !string.IsNullOrEmpty(Globals.LoggedInUser.email) && Globals.LoggedInUser.ref_member_role_id != 0 ? true : false;
        //    set => Globals.LoggedInUser.email = Globals.LoggedInUser.email;
        //}
        public HeaderViewModel()
        {
            GoHome = new RelayCommandWithParameters((param) => MainWindowViewModel.ChangeView((string)param));
            GoToLogin = new RelayCommand(() => MainWindowViewModel.ChangeView("login"));
            GoToRegister = new RelayCommand(() => MainWindowViewModel.ChangeView("register"));
            ByPassIntoBackend = new RelayCommand(() => MainWindowViewModel.ChangeView("librarian"));
            Method1();
        }

        /// <summary>
        /// Evig loop, uppdaterar från statisk variabel 1-2 gånger i sekunden
        /// Komplett retardad lösning, men jag kunde inte lösa det på något annat sätt.
        /// </summary>
        private async void Method1()
        {
            DoSomeInfiniteWork1(); //Don't use await here
            DoSomeInfiniteWork2();
        }
        public Member CurrentLoggedInUserExtended { get; set; } = Globals.LoggedInUser;
        static bool isRunning = true;
        static bool cancelwork2 = false;
        private async Task DoSomeInfiniteWork1()
        {
            while (isRunning)
            {
                CurrentLoggedInUserExtended = Globals.LoggedInUser;
                await Task.Delay(1000);
            }
        }
        
        private async Task DoSomeInfiniteWork2()
        {

            while (isRunning)
            {
                CurrentLoggedInUserExtended = Globals.LoggedInUser;
                await Task.Delay(1000);
            }

        }
    }
}
