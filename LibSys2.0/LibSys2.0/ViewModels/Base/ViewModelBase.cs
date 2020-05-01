using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LibrarySystem.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// När en property ändrats, ange namnet på propertyn som argument i denna metod för att signalera en uppdatering
        /// </summary>
        /// <param name="name"></param>
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// ...
        /// </summary>
        /// <param name="name"></param>
        public void NotifyPropertyChanged([CallerMemberName] String name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
