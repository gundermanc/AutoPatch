using System;
using System.Windows.Input;
using AutoPatcher.Abstractions;
using AutoPatcher.Models;
using AutoPatcher.Properties;

namespace AutoPatcher.Commands
{
    internal sealed class RemoveSourceItemCommand : ICommand
    {
        private readonly IAbstraction abstraction;
        private readonly PatchEditorModel model;

        public RemoveSourceItemCommand(IAbstraction abstraction, PatchEditorModel model)
        {
            this.abstraction = abstraction;
            this.model = model;
            this.model.PropertyChanged += Model_PropertyChanged;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return this.model.SelectedBuildArtifact != null &&
                this.model.SelectedSourceItem != null;
        }

        public void Execute(object parameter)
        {
            if (this.abstraction.ErrorDialogs.QuestionDialog(Resources.StringRemoveEntryPrompt))
            {
                this.model.SourceItems.Remove(this.model.SelectedSourceItem);
            }
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (this.CanExecuteChanged != null &&
                (e.PropertyName == nameof(PatchEditorModel.SelectedBuildArtifact)) ||
                (e.PropertyName == nameof(PatchEditorModel.SelectedSourceItem)))
            {
                this.CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
