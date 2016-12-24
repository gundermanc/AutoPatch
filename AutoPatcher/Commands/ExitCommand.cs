using System;
using System.Windows;
using System.Windows.Input;

namespace AutoPatcher.Commands
{
    internal sealed class ExitCommand : ICommand
    {
#pragma warning disable CS0067
        public event EventHandler CanExecuteChanged;
#pragma warning restore CS0067

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Application.Current.Shutdown();
        }
    }
}
