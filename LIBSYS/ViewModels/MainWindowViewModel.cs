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
                case "Login":
                    CurrentView.Content = new LoginView();
                    break;
                case "Admin":
                    CurrentView.Content = new AdminView();
                    break;
                default:
                    break;
            }
            
        }
    }
}
