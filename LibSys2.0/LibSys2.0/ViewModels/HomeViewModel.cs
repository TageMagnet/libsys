using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;

namespace LibSys2._0.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> LoginCommand { get; set; }
        public ReactiveCommand<Unit, Unit> RegisterCommand { get; set; }

        public string text { get; set; }

        public HomeViewModel()
        {

            LoginCommand = ReactiveCommand.Create(() => MainWindowViewModel.ChangeView("login"));
            RegisterCommand = ReactiveCommand.Create(() => MainWindowViewModel.ChangeView("register"));
        }

    }
}
