using System;
using System.Windows.Input;
using AutoPatcher.Abstractions;
using AutoPatcher.Engine.Repository;
using AutoPatcher.Models;
using AutoPatcher.Properties;
using AutoPatcher.Views;

namespace AutoPatcher.Commands
{
    internal sealed class EditSourceItemCommand : ICommand
    {
        private readonly IAbstraction abstraction;
        private readonly PatchEditorModel model;

        public EditSourceItemCommand(IAbstraction abstraction, PatchEditorModel model)
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
            var model = new PathInputModel(
                this.abstraction,
                Resources.StringAddSourceItemTitle,
                Resources.StringLocalPathContent,
                input0RelativePathPrefix: null) // TODO: update to initially open to source directory root.
            {
                Input0Text = this.model.SelectedSourceItem.LocalPath
            };

            if (new PathInputWindow() { DataContext = model }.ShowDialog() ?? false)
            {
                int index = this.model.SourceItems.IndexOf(this.model.SelectedSourceItem);
                this.model.SourceItems[index] = new SourceItem(model.Input0Text);
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
