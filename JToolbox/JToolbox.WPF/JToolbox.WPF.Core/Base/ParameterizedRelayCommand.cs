using System;
using System.Windows.Input;

namespace JToolbox.WPF.Core.Base
{
    public class ParameterizedRelayCommand
    {
        private readonly Func<object, bool> canExecute;

        private readonly Action<object> execute;

        public ParameterizedRelayCommand(Action<object> execute) : this(execute, null)
        {
        }

        public ParameterizedRelayCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public static void CanExecuteRefresh()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        public bool CanExecute(object parameter)
        {
            if (canExecute == null)
                return true;

            return canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}