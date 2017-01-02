using System;
using System.Windows.Input;
using AutoPatcher.Abstractions;
using AutoPatcher.Models;
using AutoPatcher.Properties;
using AutoPatcher.Views;

namespace AutoPatcher.Commands
{
    internal sealed class EditBinaryDirectoriesCommand : ICommand
    {
        private readonly IAbstraction abstraction;
        private readonly MainWindowModel model;

        public EditBinaryDirectoriesCommand(
            IAbstraction abstraction,
            MainWindowModel model)
        {
            this.abstraction = abstraction;
            this.model = model;
            this.model.PropertyChanged += Model_PropertyChanged;
        }

#pragma warning disable 0067
        public event EventHandler CanExecuteChanged;
#pragma warning restore 0067

        public bool CanExecute(object parameter)
        {
            return this.model.RepoConfigPath != null;
        }

        public void Execute(object parameter)
        {
            var model = new PathInputModel(
                this.abstraction,
                Resources.StringEditBinaryDirectoriesDialogTitle,
                Resources.StringLocalBinariesDirectoryContent,
                Resources.StringRemoteBinariesDirectoryContent,
                openFolderInsteadOfFile: true)
            {
                Input0Text = this.model.State.Repository.LocalBinRoot,
                Input1Text = this.model.State.CurrentRemoteBinRoot
            };

            if (new PathInputWindow() { DataContext = model }.ShowDialog() ?? false)
            {
                this.model.State.Repository.LocalBinRoot = model.Input0Text;
                this.model.State.CurrentRemoteBinRoot = model.Input1Text;

                this.model.State.RefreshBuildArtifactStates();
                this.model.RefreshBuildArtifacts();

                this.model.SaveRepository();
            }
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.model.RepoConfigPath) && this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
