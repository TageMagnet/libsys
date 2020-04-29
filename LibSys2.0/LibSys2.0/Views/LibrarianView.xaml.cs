using LibrarySystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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

namespace LibrarySystem.Views
{
    /// <summary>
    /// Interaction logic for LibrarianView.xaml
    /// </summary>
    public partial class LibrarianView : UserControl
    {
        public LibrarianView()
        {
            InitializeComponent();
            this.DataContext = new LibrarianViewModel();
        }
    }
}
