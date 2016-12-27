using System;
using System.Windows.Input;
using AutoPatcher.Config;
using AutoPatcher.Properties;

namespace AutoPatcher.PatchEditor.Commands
{
    internal sealed class EditSourceItemCommand : ICommand
    {
        private readonly PatchEditorModel model;

        public EditSourceItemCommand(PatchEditorModel model)
        {
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
            var model = new InputModel(
                Resources.StringAddSourceItemTitle,
                Resources.StringLocalPathContent)
            {
                Input0Text = this.model.SelectedSourceItem.LocalPath
            };

            var result = new InputWindow()
            {
                DataContext = model
            }.ShowDialog();

            if (result ?? false)
            {
                int index = this.model.SourceItems.IndexOf(this.model.SelectedSourceItem);
                this.model.SourceItems[index] = new SourceItemData() { LocalPath = model.Input0Text };
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
