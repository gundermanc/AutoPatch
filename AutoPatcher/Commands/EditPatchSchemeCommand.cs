using System;
using System.Windows.Input;
using AutoPatcher.Abstractions;
using AutoPatcher.Config;
using AutoPatcher.Models;
using AutoPatcher.Util;
using AutoPatcher.Views;
using System.Threading.Tasks;

namespace AutoPatcher.Commands
{
    internal sealed class EditPatchSchemeCommand : ICommand
    {
        private readonly IErrorDialogs dialogs;
        private readonly MainWindowModel model;

        public EditPatchSchemeCommand(IErrorDialogs dialogs, MainWindowModel model)
        {
            this.dialogs = dialogs;
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
                    AppConfigurationLoader.WriteAppConfiguration(this.dialogs, config);

                    // Reload settings.
                    Task.Run(async () =>
                    {
                        await this.model.UnloadAppConfigurationAsync();
                        await this.model.LoadAppConfigurationAsync(config.FilePath);
                    });
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
