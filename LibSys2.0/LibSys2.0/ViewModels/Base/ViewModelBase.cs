using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LibrarySystem.ViewModels
{
    /// <summary>
    /// Extendar till alla VMs för att slippa deklarera INotifyPropertyChanged mer än en gång
    /// Innehåller också hjälpfulla metoder
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// När en property ändrats, ange namnet på propertyn som argument i denna metod för att signalera en uppdatering
        /// </summary>
        /// <param name="name"></param>
        public void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        /// <summary>
        /// ...
        /// </summary>
        /// <param name="name"></param>
        public void NotifyPropertyChanged([CallerMemberName] String name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        //Nedanför är Commands som används i multipla ViewModels
        /// <summary>
        /// Go to specified page
        /// </summary>
        public ReactiveUI.ReactiveCommand<string, System.Reactive.Unit> GoToPage { get; set; } = ReactiveUI.ReactiveCommand.Create((string vmName) => MainWindowViewModel.ChangeView(vmName));

    }
}
