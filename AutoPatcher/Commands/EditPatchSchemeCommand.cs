using System;
using System.Windows.Input;
using AutoPatcher.Abstractions;
using AutoPatcher.Engine.Util;
using AutoPatcher.Models;
using AutoPatcher.Views;

namespace AutoPatcher.Commands
{
    internal sealed class EditPatchSchemeCommand : ICommand
    {
        private readonly IAbstraction abstraction;
        private readonly MainWindowModel model;

        public EditPatchSchemeCommand(IAbstraction abstraction, MainWindowModel model)
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
            var model = new PatchEditorModel(
                this.abstraction,
                this.model.State.Repository.BuildArtifacts.Clone());

            if (new PatchEditorWindow() { DataContext = model }.ShowDialog() ?? false)
            {
                var repo = this.model.State.Repository;

                repo.BuildArtifacts.Clear();
                repo.AddBuildArtifactsRange(model.BuildArtifacts);

                this.model.SaveRepository();
                this.model.DispatchRepositoryPropertiesChanged();
                this.model.RefreshBuildArtifacts();
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
