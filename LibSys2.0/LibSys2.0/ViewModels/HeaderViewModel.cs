using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;

namespace LibrarySystem.ViewModels
{
    public class HeaderViewModel : BaseViewModel
    {
        public ReactiveCommand<string, Unit> GoHome { get; set; } = ReactiveCommand.Create((string vmName) => MainWindowViewModel.ChangeView(vmName));
        public ReactiveCommand<Unit, Unit> GoToLogin { get; set; } = ReactiveCommand.Create(() => MainWindowViewModel.ChangeView("login"));
        public ReactiveCommand<Unit, Unit> GoToRegister { get; set; } = ReactiveCommand.Create(() => MainWindowViewModel.ChangeView("register"));
        public ReactiveCommand<Unit, Unit> ByPassIntoBackend { get; set; } = ReactiveCommand.Create(() => MainWindowViewModel.ChangeView("librarian"));

        public bool IsLoggedIn { get => !string.IsNullOrEmpty(MainWindowViewModel.CurrentLoggedInMember.role); }
        /// <summary> Extends the currently logged in member from MainWindow </summary>
        public Models.Member CurrentLoggedInMember { get => MainWindowViewModel.CurrentLoggedInMember; set => MainWindowViewModel.CurrentLoggedInMember = value; }
    }
}
