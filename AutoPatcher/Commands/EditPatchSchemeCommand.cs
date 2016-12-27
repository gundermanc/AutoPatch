using System;
using System.Windows.Input;
using AutoPatcher.Models;
using AutoPatcher.Util;
using AutoPatcher.Views;

namespace AutoPatcher.Commands
{
    internal sealed class EditPatchSchemeCommand : ICommand
    {
        private readonly MainWindowModel model;

        public EditPatchSchemeCommand(MainWindowModel model)
        {
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
            this.model.AppConfig.AccessReference((config) =>
            {
                var model = new PatchEditorModel(
                    this.model.ErrorDialogs,
                    config.Configuration.BuildArtifacts.Clone());

                if (new PatchEditorWindow() { DataContext = model }.ShowDialog() ?? false)
                {
                    config.Configuration.BuildArtifacts.Clear();
                    config.Configuration.BuildArtifacts.AddRange(model.BuildArtifacts);

                    // Persist configuration changes.
                    config.WriteToFile();
                }

                return config;
            });
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
