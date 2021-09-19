using System;
using System.Windows.Input;

namespace JToolbox.WPF.Core.Base
{
    public class RelayCommand : ICommand
    {
        private readonly Func<bool> canExecute;

        private readonly Action execute;

        public RelayCommand(Action execute) : this(execute, null)
        {
        }

        public RelayCommand(Action execute, Func<bool> canExecute)
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

            return canExecute();
        }

        public void Execute(object parameter)
        {
            execute();
        }
    }
}