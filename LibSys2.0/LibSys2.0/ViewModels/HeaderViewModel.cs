using LibrarySystem.Models;
using Org.BouncyCastle.Asn1.BC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace LibrarySystem.ViewModels
{
    public class HeaderViewModel : BaseViewModel
    {
        public RelayCommandWithParameters GoHome { get; set; }
        public RelayCommand GoToLogin { get; set; }
        public RelayCommand GoToRegister { get; set; }
        public RelayCommand ByPassIntoBackend { get; set; }
        public RelayCommandWithParameters GoToPage { get; set; }
        public RelayCommand LogoutCommand { get; set; }

        public static string IsBusy { get; set; } = "Hidden";

        public bool IsLoggedIn { get; set; } = false;

        public HeaderViewModel()
        {
                GoHome = new RelayCommandWithParameters((param) => MainWindowViewModel.ChangeView((string)param));
                GoToLogin = new RelayCommand(() => MainWindowViewModel.ChangeView("login"));
                GoToRegister = new RelayCommand(() => MainWindowViewModel.ChangeView("register"));
                ByPassIntoBackend = new RelayCommand(() => MainWindowViewModel.ChangeView("librarian"));
                GoToPage = new RelayCommandWithParameters(async (param) => await GoToProfilePage((int)param));
                LogoutCommand = new RelayCommand(async () => await LogoutCommandMethod());
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
                if (CurrentLoggedInUserExtended.ref_member_role_id != 0)
                {
                    IsLoggedIn = true;
                }
                else
                {
                    IsLoggedIn = false;
                }
                await Task.Delay(1000);
            }
            
        }

        private async Task LogoutCommandMethod()
        {
            MessageBoxResult result = MessageBox.Show("Vill du logga ut", "", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                Globals.LoggedInUser = new Member();
                MainWindowViewModel.ChangeView("home");
            }
            
        }

        private async Task GoToProfilePage(int role_id)
        {
            switch (role_id)
            {
                case 1:
                    MainWindowViewModel.ChangeView("admin");
                    break;
                case 2:
                    MainWindowViewModel.ChangeView("librarian");
                    break;
                case 3:
                    MainWindowViewModel.ChangeView("customer");
                    break;
                default:
                    break;
            }
        }

    }
}
