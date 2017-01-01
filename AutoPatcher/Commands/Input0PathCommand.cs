using System;
using System.Windows.Input;
using AutoPatcher.Abstractions;
using AutoPatcher.Models;
using AutoPatcher.Properties;

namespace AutoPatcher.Commands
{
    internal sealed class Input0PathCommand : ICommand
    {
        private readonly IAbstraction abstraction;
        private readonly PathInputModel model;

        public Input0PathCommand(IAbstraction abstraction, PathInputModel model)
        {
            this.abstraction = abstraction;
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
            if (this.model.OpenFolderInsteadOfFile)
            {
                this.model.Input0Text = this.abstraction.FileDialogs.OpenFolderDialog() ?? this.model.Input0Text;
            }
            else
            {
                this.model.Input0Text = this.abstraction.FileDialogs.OpenFileDialog(
                    Resources.StringMainWindowTitle,
                    null) ?? this.model.Input0Text;
            }
        }
    }
}
