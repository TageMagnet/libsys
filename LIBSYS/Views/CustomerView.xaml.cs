using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace LIBSYS.Views
{
    public class CustomerView : UserControl
    {
        public CustomerView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
