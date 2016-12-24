using System;
using System.Windows.Input;
using AutoPatcher.Abstractions;
using AutoPatcher.Properties;

namespace AutoPatcher.Commands
{
    internal sealed class AboutCommand : ICommand
    {
        private readonly IErrorDialogs dialogs;

        public AboutCommand(IErrorDialogs dialogs)
        {
            this.dialogs = dialogs;
        }

#pragma warning disable CS0067
        public event EventHandler CanExecuteChanged;
#pragma warning restore CS0067

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            // TODO: better dialog and version info.
            this.dialogs.InformationDialog(Resources.StringAboutMessage);
        }
    }
}
