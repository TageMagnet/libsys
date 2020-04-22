using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace LIBSYS.Views
{
    public class AdminView : UserControl
    {
        public AdminView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
