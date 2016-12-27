using System;
using System.Windows.Input;
using AutoPatcher.Abstractions;
using AutoPatcher.Models;
using AutoPatcher.Properties;

namespace AutoPatcher.Commands
{
    internal sealed class RemoveBuildArtifactCommand : ICommand
    {
        private readonly IErrorDialogs dialogs;
        private readonly PatchEditorModel model;

        public RemoveBuildArtifactCommand(IErrorDialogs dialogs, PatchEditorModel model)
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
            if (this.dialogs.QuestionDialog(Resources.StringRemoveEntryPrompt))
            {
                this.model.BuildArtifacts.Remove(this.model.SelectedBuildArtifact);
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
