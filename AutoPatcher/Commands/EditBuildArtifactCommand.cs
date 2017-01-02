using System;
using System.Windows.Input;
using AutoPatcher.Abstractions;
using AutoPatcher.Engine.Repository;
using AutoPatcher.Models;
using AutoPatcher.Properties;
using AutoPatcher.Views;

namespace AutoPatcher.Commands
{
    internal sealed class EditBuildArtifactCommand : ICommand
    {
        private readonly IAbstraction abstraction;
        private readonly PatchEditorModel model;

        public EditBuildArtifactCommand(IAbstraction abstraction, PatchEditorModel model)
        {
            this.abstraction = abstraction;
            this.model = model;
            this.model.PropertyChanged += Model_PropertyChanged;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return this.model.SelectedBuildArtifact != null;
        }

        public void Execute(object parameter)
        {
            var model = new PathInputModel(
                this.abstraction,
                Resources.StringAddBuildArtifactTitle,
                Resources.StringLocalPathContent,
                Resources.StringRemotePathContent,
                this.model.State.Repository.LocalBinRoot,
                this.model.State.CurrentRemoteBinRoot,
                input0EnsureExists: false,
                input1EnsureExists: false)
            {
                Input0Text = this.model.SelectedBuildArtifact.LocalPath,
                Input1Text = this.model.SelectedBuildArtifact.RemotePath
            };

            if (new PathInputWindow() { DataContext = model }.ShowDialog() ?? false)
            {
                int index = this.model.BuildArtifacts.IndexOf(this.model.SelectedBuildArtifact);

                var newItem = new BuildArtifact(
                    model.Input0Text,
                    model.Input1Text,
                    this.model.BuildArtifacts[index].SourceItems);

                this.model.BuildArtifacts[index] = newItem;
            }
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PatchEditorModel.SelectedBuildArtifact) && this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
