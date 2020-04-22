using Avalonia.Controls;
using LIBSYS.Views;
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
        }
    }
}
