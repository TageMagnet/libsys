using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using LIBSYS.ViewModels;

namespace LIBSYS.Views
{
    public class LoginView : UserControl
    {
        public LoginView()
        {
            
            this.InitializeComponent();
            this.DataContext = new LoginViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
