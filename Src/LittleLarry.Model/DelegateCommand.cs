using System;
using System.Windows.Input;

namespace LittleLarry.Model
{
    public class DelegateCommand : ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;

        public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        public DelegateCommand(Action<object> execute)
        {
            _execute = execute;
            _canExecute = o => true;
        }

        public event EventHandler CanExecuteChanged;
        internal void OnCanExecuteChanged() =>
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public bool CanExecute(object parameter)
            => _canExecute(parameter);

        public void Execute(object parameter)
        {
            if (CanExecute(parameter)) _execute(parameter);
        }
    }
}
