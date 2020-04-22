﻿using LIBSYS.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;

namespace LIBSYS.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> LoginCommand { get; set; }
        public ReactiveCommand<Unit,Unit> RegisterCommand { get; set; }

        public string text { get; set; }

        public HomeViewModel()
        {
            text = "Hello hej";
            LoginCommand = ReactiveCommand.Create(() => MainWindowViewModel.ChangeView("Login"));
            RegisterCommand = ReactiveCommand.Create(() => MainWindowViewModel.ChangeView("Register"));
        }

    }
}