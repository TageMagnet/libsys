﻿using LibrarySystem.Views;
using System.Windows.Controls;

namespace LibrarySystem.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        /// <summary>
        /// Current View/ViewModel
        /// </summary>
        public static UserControl CurrentView { get; set; }

        /// <summary>
        /// Currently logged in member
        /// </summary>
        public static Models.Member CurrentLoggedInMember { get; set; } = new Models.Member();

        public MainWindowViewModel()
        {
            CurrentView = new HomeView();
        }

        /// <summary>
        /// Metod för att byta viewmodel/datacontext som "UserControl" hänger ihop med
        /// </summary>
        /// <param name="view"></param>
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
