using System;
using System.Windows.Input;
using AutoPatcher.Config;
using AutoPatcher.Properties;

namespace AutoPatcher.PatchEditor.Commands
{
    internal sealed class EditBuildArtifactCommand : ICommand
    {
        private readonly PatchEditorModel model;

        public EditBuildArtifactCommand(PatchEditorModel model)
        {
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
            var model = new InputModel(
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

                this.model.BuildArtifacts[index] = new BuildArtifactData()
                {
                    LocalPath = model.Input0Text,
                    RemotePath = model.Input1Text
                };
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
