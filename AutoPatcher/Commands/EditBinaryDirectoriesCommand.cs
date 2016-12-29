using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoPatcher.Abstractions;
using AutoPatcher.Config;
using AutoPatcher.Models;
using AutoPatcher.Properties;
using AutoPatcher.Views;

namespace AutoPatcher.Commands
{
    internal sealed class EditBinaryDirectoriesCommand : ICommand
    {
        private readonly IErrorDialogs errDialogs;
        private readonly IFileDialogs dialogs;
        private readonly MainWindowModel model;

        public EditBinaryDirectoriesCommand(
            IErrorDialogs errDialogs,
            IFileDialogs dialogs,
            MainWindowModel model)
        {
            this.dialogs = dialogs;
            this.model = model;
            this.model.PropertyChanged += Model_PropertyChanged;
        }

#pragma warning disable 0067
        public event EventHandler CanExecuteChanged;
#pragma warning restore 0067

        public bool CanExecute(object parameter)
        {
            return this.model.RepoConfigPath != null;
        }

        public void Execute(object parameter)
        {
            this.model.AppConfig.AccessReference((config) =>
            {
                var model = new PathInputModel(
                    this.dialogs,
                    Resources.StringEditBinaryDirectoriesDialogTitle,
                    Resources.StringLocalBinariesDirectoryContent,
                    Resources.StringRemoteBinariesDirectoryContent,
                    openFolderInsteadOfFile: true)
                {
                    Input0Text = config.Configuration.LocalBinRoot,
                    Input1Text = config.Configuration.RemoteBinRoot
                };

                if (new PathInputWindow() { DataContext = model }.ShowDialog() ?? false)
                {
                    config.Configuration.LocalBinRoot = model.Input0Text;
                    config.Configuration.RemoteBinRoot = model.Input1Text;

                    Task.Run(() => AppConfigurationLoader.WriteAppConfiguration(this.errDialogs, config));
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
