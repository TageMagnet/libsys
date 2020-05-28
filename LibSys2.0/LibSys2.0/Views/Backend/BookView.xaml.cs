﻿using LibrarySystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibrarySystem
{
    /// <summary>
    /// Interaction logic for BookView.xaml
    /// </summary>
    public partial class BookView : UserControl
    {
        public BookView()
        {
            
            InitializeComponent();
            this.DataContext = new BookViewModel();
            Loaded += MyWindowLoaded;
            HeaderViewModel.IsBusy = "Visible";
        }

        private void MyWindowLoaded(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Laddat");
            HeaderViewModel.IsBusy = "Hidden";
        }
    }
}
