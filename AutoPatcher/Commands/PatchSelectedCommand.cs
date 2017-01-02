using System;
using System.Windows.Input;
using AutoPatcher.Abstractions;
using AutoPatcher.Models;

namespace AutoPatcher.Commands
{
    internal sealed class PatchSelectedCommand : ICommand
    {
        private readonly IAbstraction abstraction;
        private readonly MainWindowModel model;

        public PatchSelectedCommand(IAbstraction abstraction, MainWindowModel model)
        {
            this.abstraction = abstraction;
            this.model = model;
            this.model.PropertyChanged += Model_PropertyChanged;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return this.model.RepoConfigPath != null;
        }

        public void Execute(object parameter)
        {
            this.model.State.PatchBuildArtifacts(this.model.SelectedBuildArtifacts);
            this.model.RefreshBuildArtifacts();
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
