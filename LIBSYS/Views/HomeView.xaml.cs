﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using LIBSYS.ViewModels;

namespace LIBSYS.Views
{
    public class HomeView : UserControl
    {
        public HomeView()
        {
            this.InitializeComponent();
            this.DataContext = new HomeViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}