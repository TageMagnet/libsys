using Avalonia.Controls;
using LIBSYS.Models;
using LIBSYS.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIBSYS.ViewModels
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
