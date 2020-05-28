using System;
using LibrarySystem.Views;
using System.Windows.Controls;
using System.Threading;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace LibrarySystem.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        /// <summary>
        /// Current View/ViewModel
        /// </summary>
        public static UserControl CurrentView { get; set; }

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
                    CurrentView.Content = new LibrarianView();
                    break;
                case "librarian":
                    CurrentView.Content = new LibrarianView();
                    break;
                case "customer":
                    CurrentView.Content = new CustomerView();
                    break;
                case "register":
                    CurrentView.Content = new RegisterView();
                    break;
                default:
                    break;
            }

        }
    }
}
