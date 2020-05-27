using LibrarySystem.ViewModels.Backend;
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
using System.Windows.Shapes;

namespace LibrarySystem.Views.Backend
{
    /// <summary>
    /// Interaction logic for BookReportView.xaml
    /// </summary>
    public partial class BookReportView : Window
    {
        public BookReportView()
        {
            InitializeComponent();
            //this.DataContext = new BookReportViewModel();
        }
    }
}
