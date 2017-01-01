using System;
using System.Windows.Input;
using AutoPatcher.Abstractions;
using AutoPatcher.Models;
using AutoPatcher.Properties;

namespace AutoPatcher.Commands
{
    internal sealed class Input1PathCommand : ICommand
    {
        private readonly IAbstraction abstraction;
        private readonly PathInputModel model;

        public Input1PathCommand(IAbstraction abstraction, PathInputModel model)
        {
            this.abstraction = abstraction;
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
            if (this.model.OpenFolderInsteadOfFile)
            {
                this.model.Input1Text = this.abstraction.FileDialogs.OpenFolderDialog() ?? this.model.Input1Text;
            }
            else
            {
                this.model.Input1Text = this.abstraction.FileDialogs.OpenFileDialog(
                    Resources.StringMainWindowTitle,
                    null) ?? this.model.Input1Text;
            }
        }
    }
}
