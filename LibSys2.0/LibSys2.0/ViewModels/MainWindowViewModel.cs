using LibrarySystem.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace LibrarySystem.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public static UserControl CurrentView { get; set; }
        public MainWindowViewModel()
        {
            CurrentView = new HomeView();
            //this.RaisePropertyChanged(nameof(CurrentView));
        }

        public static void ChangeView(string view)
        {
            switch (view)
            {
                case "home":
                    CurrentView.Content = new HomeView();
                    break;
                case "login":
                    CurrentView.Content = new LoginView();
                    break;
                case "admin":
                    CurrentView.Content = new AdminView();
                    break;
                case "librarian":
                    CurrentView.Content = new LibrarianView();
                    break;
                case "customer":
                    CurrentView.Content = new CustomerView();
                    break;
                default:
                    break;
            }

        }
    }
}
