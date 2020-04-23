using LIBSYS.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;

namespace LIBSYS.ViewModels
{
    public class LoginViewModel: ViewModelBase
    {
        public Repository repo = new Repository();

        public ReactiveCommand<Unit, Unit> AdminCommand { get; set; }
        public LoginViewModel()
        {
            AdminCommand = ReactiveCommand.Create(() => MainWindowViewModel.ChangeView("Admin"));
        }

        //Skapa loginmetod

    }

}
