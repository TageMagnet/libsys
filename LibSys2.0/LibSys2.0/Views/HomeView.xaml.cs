using LibrarySystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
            this.DataContext = new HomeViewModel();
        }


        /// <summary>
        /// <para>##########################</para>
        /// <para>##########################</para>
        /// <para>##########################</para>
        /// 
        /// Breaking MVVM, could not figure how to make an storyboard animation only run once through pure XAML.
        /// 
        /// <para>##########################</para>
        /// <para>##########################</para>
        /// <para>##########################</para>
        /// </summary>
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            // Disable button.triggers
            Application.Current.Dispatcher.Invoke(async () =>
            {
                Task delay = Task.Delay(500);
                await delay;
                button.Triggers.Clear();
            });
        }
    }
}
