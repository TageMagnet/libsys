﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace LIBSYS.Views
{
    public class LoginView : UserControl
    {
        public LoginView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
