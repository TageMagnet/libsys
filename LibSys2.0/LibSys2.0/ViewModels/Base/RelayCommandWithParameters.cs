using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LibrarySystem
{
    public class RelayCommandWithParameters : ICommand
    {
        /// <summary>
        /// The event that's fired when the <see cref="CanExecute(object)"/>  value has changed
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommandWithParameters(Action<object> execute) : this(execute, null)
        {
        }
        public RelayCommandWithParameters(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null ? true : _canExecute(parameter);

        public void Execute(object parameter) => _execute(parameter);
        //private Action<object> action;
        //public event EventHandler CanExecuteChanged = (sender, e) => { };

        //public RelayCommandWithParameters(Action<object> action)
        //{
        //    this.action = action;
        //}

        //public event EventHandler CanExecuteChanged
        //{
        //    add { CommandManager.RequerySuggested += value; }
        //    remove { CommandManager.RequerySuggested -= value; }
        //}

        //public bool CanExecute(object parameter) => true;

        //public void Execute(object parameter) => action(parameter);
    }
}
