using LibrarySystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
            // Sätt muspekare till laddningssnurra
            Application.Current.Dispatcher.Invoke(() => Mouse.OverrideCursor = Cursors.Wait);
            Loaded += BookView_Loaded;
            InitializeComponent();
            this.DataContext = new BookViewModel();

        }

        private void BookView_Loaded(object sender, RoutedEventArgs e)
        {
            // Nolställ mus till standard
            Application.Current.Dispatcher.Invoke(() => Mouse.OverrideCursor = null);
        }
    }
}
