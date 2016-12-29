using System;
using System.Windows;
using System.Windows.Input;
using AutoPatcher.Abstractions;
using AutoPatcher.Models;
using AutoPatcher.Properties;

namespace AutoPatcher.Commands
{
    internal sealed class Input1PathCommand : ICommand
    {
        private readonly IFileDialogs dialogs;
        private readonly PathInputModel model;

        public Input1PathCommand(IFileDialogs dialogs, PathInputModel model)
        {
            this.dialogs = dialogs;
            this.model = model;
        }

#pragma warning disable 0067
        public event EventHandler CanExecuteChanged;
#pragma warning restore 0067

        public bool CanExecute(object parameter)
        {
            return this.model.IsInput1Enabled;
        }

        public void Execute(object parameter)
        {
            this.model.Input1Text = this.dialogs.OpenFileDialog(
                Resources.StringMainWindowTitle,
                null);
        }
    }
}
