using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using LIBSYS.ViewModels;

namespace LIBSYS.Views
{
    public class LibrarianView : UserControl
    {
        public LibrarianView()
        {
            this.InitializeComponent();
            this.DataContext = new LibrarianViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
