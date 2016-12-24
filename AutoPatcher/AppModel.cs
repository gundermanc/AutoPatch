using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AutoPatcher.Abstractions;
using AutoPatcher.Commands;
using AutoPatcher.Config;
using AutoPatcher.Util;

namespace AutoPatcher
{
    internal sealed class AppModel : MainWindowModelBase
    {
        #region Private fields

        private bool isLoadingAppConfiguration = false;

        #endregion

        public AppModel()
        {
            this.OpenRepoCommand = new OpenRepoCommand(this);
            this.NewRepoCommand = new NewRepoCommand(this);
            this.CloseRepoCommand = new CloseRepoCommand(this);
            this.AboutCommand = new AboutCommand(this.ErrorDialogs);
            this.EditPatchSchemeCommand = new EditPatchSchemeCommand(this);
        }

        #region App Submodels

        public IErrorDialogs ErrorDialogs { get; } = new ErrorDialogs();

        public IFileDialogs FileDialogs { get; } = new FileDialogs();

        // Locked reference is my lazy solution to the concurrency issues that arise here..
        // be sure not to allow the reference to escape the lambda.
        private LockedReference<AppConfiguration> AppConfig { get; } = new LockedReference<AppConfiguration>();

        #endregion

        #region Commands

        public ICommand OpenRepoCommand { get; }

        public ICommand NewRepoCommand { get; }

        public ICommand CloseRepoCommand { get; }

        public ICommand ExitCommand { get; } = new ExitCommand();

        public ICommand AboutCommand { get; }

        public ICommand EditPatchSchemeCommand { get; }

        #endregion

        #region App Model Concepts

        public bool IsModifyingLoadedAppConfiguration
        {
            get
            {
                return this.isLoadingAppConfiguration;
            }

            set
            {
                if (this.isLoadingAppConfiguration != value)
                {
                    this.isLoadingAppConfiguration = value;
                    DispatchPropertyChanged(nameof(this.IsModifyingLoadedAppConfiguration));

                    this.IsBusy = value;
                }
            }
        }

        #endregion

        public async Task CreateAndLoadAppConfigurationAsync(string filePath)
        {
            // Assert only because this is verified by the command.
            Debug.Assert(!this.IsModifyingLoadedAppConfiguration);

            bool isLoaded = false;

            Application.Current.Dispatcher.Invoke(() => this.IsModifyingLoadedAppConfiguration = true);

            await UnloadAppConfigurationAsync();

            await Task.Run(() =>
            {
                this.AppConfig.AccessReference((config) =>
                {
                    var newConfig = AppConfiguration.Create(filePath);

                    AppConfigurationLoader.WriteAppConfiguration(this.ErrorDialogs, newConfig);

                    isLoaded = newConfig != null;

                    // Update reference to new app configuration.
                    return newConfig;
                });
            });
            
            Application.Current.Dispatcher.Invoke(() =>
            {
                this.IsModifyingLoadedAppConfiguration = false;

                if (isLoaded)
                {
                    this.RepoConfigPath = filePath;
                }
            });
        }

        public async Task LoadAppConfigurationAsync(string filePath)
        {
            // Assert only because this is verified by the command.
            Debug.Assert(!this.IsModifyingLoadedAppConfiguration);

            bool isLoaded = false;

            Application.Current.Dispatcher.Invoke(() => this.IsModifyingLoadedAppConfiguration = true);

            await Task.Run(() =>
            {
                this.AppConfig.AccessReference((config) =>
                {
                    var newConfig = AppConfigurationLoader.CreateAppConfigurationFromFile(this.ErrorDialogs, filePath);

                    isLoaded = newConfig != null;

                    return newConfig;
                });
            });

            Application.Current.Dispatcher.Invoke(() =>
            {
                this.IsModifyingLoadedAppConfiguration = false;

                if (isLoaded)
                {
                    this.RepoConfigPath = filePath;
                }
            });
        }

        public async Task UnloadAppConfigurationAsync()
        {
            Application.Current.Dispatcher.Invoke(() => this.IsModifyingLoadedAppConfiguration = true);

            await Task.Run(() =>
            {
                this.AppConfig.AccessReference((config) =>
                {
                    if (config != null)
                    {
                        AppConfigurationLoader.WriteAppConfiguration(this.ErrorDialogs, config);
                    }

                    // Update reference to null.
                    return null;
                });
            });

            Application.Current.Dispatcher.Invoke(() =>
            {
                this.IsModifyingLoadedAppConfiguration = false;
                this.RepoConfigPath = null;
            });
        }
    }
}
