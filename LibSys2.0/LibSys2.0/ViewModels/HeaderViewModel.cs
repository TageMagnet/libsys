using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.ViewModels
{
    public class HeaderViewModel : BaseViewModel
    {
        public RelayCommandWithParameters GoHome { get; set; }
        public RelayCommand GoToLogin { get; set; }
        public RelayCommand GoToRegister { get; set; }
        public RelayCommand ByPassIntoBackend { get; set; }
        public bool IsLoggedIn { get => !string.IsNullOrEmpty(MainWindowViewModel.CurrentLoggedInMember.role); }
        /// <summary> Extends the currently logged in member from MainWindow </summary>
        public Models.Member CurrentLoggedInMember { get => MainWindowViewModel.CurrentLoggedInMember; set => MainWindowViewModel.CurrentLoggedInMember = value; }

        public HeaderViewModel()
        {
            GoHome = new RelayCommandWithParameters((param) => MainWindowViewModel.ChangeView((string)param));
            GoToLogin = new RelayCommand(() => MainWindowViewModel.ChangeView("login")); 
            GoToRegister = new RelayCommand(() => MainWindowViewModel.ChangeView("register"));
            ByPassIntoBackend = new RelayCommand(() => MainWindowViewModel.ChangeView("librarian"));
        }
    }
}
