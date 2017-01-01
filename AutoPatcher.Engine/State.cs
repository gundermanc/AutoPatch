using System;
using AutoPatcher.Engine.Repository;
using AutoPatcher.Engine.Abstractions;
using AutoPatcher.Engine.Util;
using System.Threading.Tasks;

namespace AutoPatcher.Engine
{
    public sealed class State : IState
    {
        private const string CurrentRemoteBinPathRootSettingKey = "AutoPatch_Engine_CurrentRemoteBinPathRoot";
        private string currentRemoteBinRoot;

        public State(IErrorDialogs errorDialogs, IFileDialogs fileDialogs, ISettingsManager settingsManager)
        {
            Verify.IsNotNull(errorDialogs, nameof(errorDialogs));
            Verify.IsNotNull(fileDialogs, nameof(fileDialogs));
            Verify.IsNotNull(settingsManager, nameof(settingsManager));

            this.ErrorDialogs = errorDialogs;
            this.FileDialogs = fileDialogs;
            this.SettingsManager = settingsManager;

            this.CurrentRemoteBinRoot = settingsManager.GetSettingsValue(CurrentRemoteBinPathRootSettingKey);
        }

        public string CurrentRemoteBinRoot
        {
            get
            {
                return this.currentRemoteBinRoot;
            }

            set
            {
                if (this.currentRemoteBinRoot != value)
                {
                    this.currentRemoteBinRoot = value;
                    this.SettingsManager.SetSettingsValue(CurrentRemoteBinPathRootSettingKey, value);
                    this.SettingsManager.CommitChanges();
                }
            }
        }

        public IRepository Repository { get; private set; }

        private IErrorDialogs ErrorDialogs { get; }

        private IFileDialogs FileDialogs { get; }

        private ISettingsManager SettingsManager { get; }

        public async Task CreateAndLoadRepositoryAsync(string filePath)
        {
            UnloadRepository();

            this.Repository = RepositoryLoader.CreateEmptyFile(this.ErrorDialogs, filePath);

            await SaveRepositoryAsync();
        }

        public async Task LoadRepositoryAsync(string filePath)
        {
            this.UnloadRepository();
            this.Repository = await Task.Run(() => RepositoryLoader.CreateFromFile(this.ErrorDialogs, filePath));
        }

        public async Task SaveRepositoryAsync()
        {
            if (this.Repository == null)
            {
                throw new InvalidOperationException("No repository loaded");
            }

            await Task.Run(() => RepositoryLoader.WriteToFile(this.ErrorDialogs, this.Repository));
        }

        public void UnloadRepository()
        {
            if (this.Repository != null)
            {
                this.Repository = null;
            }
        }
    }
}
