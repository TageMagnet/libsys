using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reflection;
using System.Text;

namespace LIBSYS.ViewModels
{
    public class HomeViewModel
    {
        public ReactiveCommand<Unit, Unit> LoginCommand { get; set; }

        public HomeViewModel()
        {
            //LoginCommand.Subscribe(() => MainWindowViewModel.CurrentView = new LoginView());
        }

        public void LoginMethod()
        {

        }
    }
}
