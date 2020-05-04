using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;

namespace LibrarySystem.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        /// <summary>
        /// Kopplad till "Login"-<see cref="System.Windows.Controls.Button"/>
        /// </summary>
        public ReactiveCommand<Unit, Unit> LoginCommand { get; set; }
        /// <summary>
        /// Register -//-
        /// </summary>
        public ReactiveCommand<Unit, Unit> RegisterCommand { get; set; }
        /// <summary>
        /// Demo för propertychanged, kan tas bort
        /// </summary>
        public ReactiveCommand<Unit, Unit> TestChange { get; set; }

        public string Text { get; set; } = "Hello world";

        public HomeViewModel()
        {
            LoginCommand = ReactiveCommand.Create(() => MainWindowViewModel.ChangeView("login"));
            RegisterCommand = ReactiveCommand.Create(() => MainWindowViewModel.ChangeView("register"));
            TestChange = ReactiveCommand.Create(() => {
                Text = "I WAS UPDATED";
                OnPropertyChanged("Text");
            });
        }

    }
}
