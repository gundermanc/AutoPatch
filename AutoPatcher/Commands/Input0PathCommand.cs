using System;
using System.Windows;
using System.Windows.Input;
using AutoPatcher.Abstractions;
using AutoPatcher.Models;
using AutoPatcher.Properties;

namespace AutoPatcher.Commands
{
    internal sealed class Input0PathCommand : ICommand
    {
        private readonly IFileDialogs dialogs;
        private readonly PathInputModel model;

        public Input0PathCommand(IFileDialogs dialogs, PathInputModel model)
        {
            this.dialogs = dialogs;
            this.model = model;
        }

#pragma warning disable 0067
        public event EventHandler CanExecuteChanged;
#pragma warning restore 0067

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.model.Input0Text = this.dialogs.OpenFileDialog(
                Resources.StringMainWindowTitle,
                null);
        }
    }
}
