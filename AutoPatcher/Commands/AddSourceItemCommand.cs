using System;
using System.ComponentModel;
using System.Windows.Input;
using AutoPatcher.Abstractions;
using AutoPatcher.Config;
using AutoPatcher.Models;
using AutoPatcher.Properties;
using AutoPatcher.Views;


namespace AutoPatcher.Commands
{
    internal sealed class AddSourceItemCommand : ICommand
    {
        private readonly IFileDialogs dialogs;
        private readonly PatchEditorModel model;

        public AddSourceItemCommand(IFileDialogs dialogs, PatchEditorModel model)
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
                Resources.StringLocalPathContent);

            var result = new PathInputWindow() { DataContext = model }.ShowDialog();

            if (result ?? false)
            {
                this.model.SourceItems.Add(new SourceItemData() { LocalPath = model.Input0Text });
            }
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PatchEditorModel.SelectedBuildArtifact) && this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
