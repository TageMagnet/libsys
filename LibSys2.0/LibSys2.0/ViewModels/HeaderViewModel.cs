﻿using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;

namespace LibrarySystem.ViewModels
{
    public class HeaderViewModel : BaseViewModel
    {
        public ReactiveCommand<string, Unit> GoHomeCommand { get; set; } = ReactiveCommand.Create((string vmName) => MainWindowViewModel.ChangeView(vmName));
        public ReactiveCommand<Unit, Unit> LoginCommand { get; set; } = ReactiveCommand.Create(() => MainWindowViewModel.ChangeView("librarian"));
        public ReactiveCommand<Unit, Unit> RegisterCommand { get; set; } = ReactiveCommand.Create(() => MainWindowViewModel.ChangeView("register"));

        public bool IsLoggedIn { get => !string.IsNullOrEmpty(MainWindowViewModel.CurrentLoggedInMember.role); }
        /// <summary> Extends the currently logged in member from MainWindow </summary>
        public Models.Member CurrentLoggedInMember { get => MainWindowViewModel.CurrentLoggedInMember; set => MainWindowViewModel.CurrentLoggedInMember = value; }


    }
}
