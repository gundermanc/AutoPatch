using System;
using System.Windows.Input;
using AutoPatcher.Config;
using AutoPatcher.Models;
using AutoPatcher.Properties;
using AutoPatcher.Views;
using AutoPatcher.Abstractions;

namespace AutoPatcher.Commands
{
    internal sealed class EditBuildArtifactCommand : ICommand
    {
        private readonly IFileDialogs dialogs;
        private readonly PatchEditorModel model;

        public EditBuildArtifactCommand(IFileDialogs dialogs, PatchEditorModel model)
        {
            this.dialogs = dialogs;
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
                this.dialogs,
                Resources.StringAddBuildArtifactTitle,
                Resources.StringLocalPathContent,
                Resources.StringRemotePathContent)
            {
                Input0Text = this.model.SelectedBuildArtifact.LocalPath,
                Input1Text = this.model.SelectedBuildArtifact.RemotePath
            };

            var result = new InputWindow()
            {
                DataContext = model
            }.ShowDialog();

            if (result ?? false)
            {
                int index = this.model.BuildArtifacts.IndexOf(this.model.SelectedBuildArtifact);

                var newItem = new BuildArtifactData()
                {
                    LocalPath = model.Input0Text,
                    RemotePath = model.Input1Text
                };

                newItem.SourceItems.AddRange(this.model.BuildArtifacts[index].SourceItems);
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
