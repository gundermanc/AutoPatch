using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using AutoPatcher.Abstractions;
using AutoPatcher.Commands;
using AutoPatcher.Engine;
using AutoPatcher.Engine.Repository;
using AutoPatcher.Properties;

namespace AutoPatcher.Models
{
    internal sealed class MainWindowModel : ModelBase
    {
        #region Private fields

        private bool isBusy = false;
        private Cursor cursor = Cursors.Arrow;
        private string statusBarText = Resources.StringStatusBarReady;
        private bool isLoadingAppConfiguration;
        private ObservableCollection<BuildArtifact> buildArtifacts;

        #endregion

        public MainWindowModel()
        {
            this.OpenRepoCommand = new OpenRepoCommand(this);
            this.NewRepoCommand = new NewRepoCommand(this);
            this.CloseRepoCommand = new CloseRepoCommand(this);
            this.AboutCommand = new AboutCommand(this.Abstraction);
            this.EditPatchSchemeCommand = new EditPatchSchemeCommand(this.Abstraction, this);
            this.PatchSelectedCommand = new PatchSelectedCommand(this.Abstraction, this);
            this.EditBinaryDirectoriesCommand = new EditBinaryDirectoriesCommand(this.Abstraction, this);

            this.State = new State(
                this.Abstraction.ErrorDialogs,
                this.Abstraction.FileDialogs,
                this.Abstraction.SettingsManager);
        }

        #region App Submodels

        public IAbstraction Abstraction = new Abstraction();

        public IState State { get; }

        #endregion

        #region Commands

        public ICommand OpenRepoCommand { get; }

        public ICommand NewRepoCommand { get; }

        public ICommand CloseRepoCommand { get; }

        public ICommand ExitCommand { get; } = new ExitCommand();

        public ICommand AboutCommand { get; }

        public ICommand EditPatchSchemeCommand { get; }

        public ICommand PatchSelectedCommand { get; }

        public ICommand EditBinaryDirectoriesCommand { get; }

        #endregion

        #region App Model Properties

        public bool IsBusy
        {
            get
            {
                return this.isBusy;
            }

            set
            {
                if (value != this.isBusy)
                {
                    this.isBusy = value;
                    DispatchPropertyChanged(nameof(this.IsBusy));

                    this.Cursor = this.IsBusy ? Cursors.Wait : Cursors.Arrow;
                    this.StatusBarText = this.IsBusy ? Resources.StringStatusBarWorking : Resources.StringStatusBarReady;
                }
            }
        }


        public ObservableCollection<BuildArtifact> BuildArtifacts
        {
            get
            {
                return this.buildArtifacts;
            }

            set
            {
                if (this.buildArtifacts != value)
                {
                    buildArtifacts = value;
                    DispatchPropertyChanged(nameof(this.BuildArtifacts));
                }
            }
        }

        public IList<BuildArtifact> SelectedBuildArtifacts { get; } = new List<BuildArtifact>();

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

        public string RepoConfigPath => this.State.Repository?.RepositoryConfigurationFilePath;

        public string LocalBinRoot => this.State.Repository?.LocalBinRoot;

        public string RemoteBinRoot => this.State?.CurrentRemoteBinRoot;

        #endregion

        #region App Model Methods

        public void CreateAndLoadRepository(string filePath)
        {
            // Assert only because this is verified by the command.
            Debug.Assert(!this.IsModifyingLoadedAppConfiguration);

            this.IsModifyingLoadedAppConfiguration = true;

            this.State.CreateAndLoadRepositoryAsync(filePath).ContinueWith((task) =>
            {
                if (this.State.Repository != null)
                {
                    this.BuildArtifacts = new ObservableCollection<BuildArtifact>();
                    DispatchRepositoryPropertiesChanged();
                }

                this.IsModifyingLoadedAppConfiguration = false;
            });
        }

        public void LoadRepository(string filePath)
        {
            // Assert only because this is verified by the command.
            Debug.Assert(!this.IsModifyingLoadedAppConfiguration);

            this.IsModifyingLoadedAppConfiguration = true;

            this.State.LoadRepositoryAsync(filePath).ContinueWith((task) =>
            {
                if (this.State.Repository != null)
                {
                    RefreshBuildArtifacts();
                    DispatchRepositoryPropertiesChanged();
                }

                this.IsModifyingLoadedAppConfiguration = false;
            });
        }

        public void UnloadRepository()
        {
            // Assert only because this is verified by the command.
            Debug.Assert(!this.IsModifyingLoadedAppConfiguration);

            this.IsModifyingLoadedAppConfiguration = true;

            this.State.UnloadRepository();

            this.BuildArtifacts = null;
            DispatchRepositoryPropertiesChanged();
            this.IsModifyingLoadedAppConfiguration = false;
        }

        public void SaveRepository()
        {
            Debug.Assert(!this.IsModifyingLoadedAppConfiguration);

            this.IsModifyingLoadedAppConfiguration = true;

            this.State.SaveRepositoryAsync().ContinueWith((task) => { this.IsModifyingLoadedAppConfiguration = false; });
        }

        public void DispatchRepositoryPropertiesChanged()
        {
            this.DispatchPropertyChanged(nameof(this.RepoConfigPath));
            this.DispatchPropertyChanged(nameof(this.LocalBinRoot));
            this.DispatchPropertyChanged(nameof(this.RemoteBinRoot));
        }

        public void RefreshBuildArtifacts()
        {
            this.BuildArtifacts = new ObservableCollection<BuildArtifact>(this.State.Repository.BuildArtifacts);
        }

        #endregion

        #region Window Properties and Events

        public Cursor Cursor
        {
            get
            {
                return this.cursor;
            }

            set
            {
                if (this.cursor != value)
                {
                    cursor = value;
                    DispatchPropertyChanged(nameof(this.Cursor));
                }
            }
        }

        public string StatusBarText
        {
            get
            {
                return this.statusBarText;
            }

            set
            {
                if (this.statusBarText != value)
                {
                    this.statusBarText = value;
                    DispatchPropertyChanged(nameof(this.StatusBarText));
                }
            }
        }

        #endregion
    }
}
