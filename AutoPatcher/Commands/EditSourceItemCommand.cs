using System;
using System.Windows.Input;
using AutoPatcher.Config;
using AutoPatcher.Models;
using AutoPatcher.Properties;
using AutoPatcher.Views;
using AutoPatcher.Abstractions;

namespace AutoPatcher.Commands
{
    internal sealed class EditSourceItemCommand : ICommand
    {
        private readonly IFileDialogs dialogs;
        private readonly PatchEditorModel model;

        public EditSourceItemCommand(IFileDialogs dialogs, PatchEditorModel model)
        {
            this.dialogs = dialogs;
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
            var model = new PathInputModel(
                this.dialogs,
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
