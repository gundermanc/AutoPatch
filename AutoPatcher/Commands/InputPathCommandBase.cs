using System;
using System.Windows.Input;
using AutoPatcher.Abstractions;
using AutoPatcher.Models;
using AutoPatcher.Properties;

namespace AutoPatcher.Commands
{
    internal abstract class InputPathCommandBase : ICommand
    {
        protected readonly IAbstraction abstraction;
        protected readonly PathInputModel model;

        public InputPathCommandBase(IAbstraction abstraction, PathInputModel model)
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

        public abstract void Execute(object parameter);

        protected void ExecuteForInput(
            ref string inputText,
            string inputRelativePathPrefix,
            bool inputEnsureExists)
        {
            if (this.model.OpenFolderInsteadOfFile)
            {
                inputText = this.abstraction.FileDialogs.OpenFolderDialog() ?? inputText;
            }
            else
            {
                var absolutePath = this.abstraction.FileDialogs.OpenFileDialog(
                    Resources.StringMainWindowTitle,
                    null,
                    inputRelativePathPrefix,
                    inputEnsureExists);

                if (absolutePath != null)
                {
                    if ((inputRelativePathPrefix != null) &&
                        !absolutePath.StartsWith(inputRelativePathPrefix))
                    {
                        this.abstraction.ErrorDialogs.WarningDialog(Resources.StringPathInputPathMustBeRelative);
                        return;
                    }

                    inputText = absolutePath.Substring(inputRelativePathPrefix.Length).TrimStart('\\');
                }
            }
        }
    }
}
