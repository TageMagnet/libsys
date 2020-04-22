using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace LIBSYS.Views
{
    public class LibrarianView : UserControl
    {
        public LibrarianView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
