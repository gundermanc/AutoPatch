using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AutoPatcher.Engine.Abstractions;
using AutoPatcher.Engine.Properties;
using AutoPatcher.Engine.Repository;
using AutoPatcher.Engine.Util;

namespace AutoPatcher.Engine
{
    public sealed class State : IState
    {
        private const string CurrentRemoteBinPathRootSettingKey = "AutoPatch_Engine_CurrentRemoteBinPathRoot";
        private const string EmptyRevisionSuffix = ".empty";
        private const string StockRevisionSuffix = ".stockrevision";
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

        public void PatchBuildArtifacts(IEnumerable<BuildArtifact> buildArtifacts)
        {
            Verify.IsNotNull(buildArtifacts, nameof(buildArtifacts));

            if (!this.IsInPatchableState())
            {
                return;
            }

            foreach (var artifact in buildArtifacts)
            {
                PatchBuildArtifact(artifact);
            }
        }

        public void RevertBuildArtifacts(IEnumerable<BuildArtifact> buildArtifacts)
        {
            Verify.IsNotNull(buildArtifacts, nameof(buildArtifacts));

            if (!this.IsInPatchableState(true))
            {
                return;
            }

            foreach (var artifact in buildArtifacts)
            {
                RevertBuildArtifact(artifact);
            }
        }

        private bool IsInPatchableState(bool isPatchRevert = false)
        {
            if (this.Repository == null)
            {
                throw new InvalidOperationException("No repository loaded");
            }

            if (!isPatchRevert && !Directory.Exists(this.Repository.LocalBinRoot))
            {
                this.ErrorDialogs.ErrorDialog(
                    string.Format(
                        Resources.StringPatchFailureInvalidLocalBinRoot,
                        this.CurrentRemoteBinRoot));
                return false;
            }

            if (!Directory.Exists(this.CurrentRemoteBinRoot))
            {
                this.ErrorDialogs.ErrorDialog(
                    string.Format(
                        Resources.StringPatchFailureInvalidRemoteBinRoot,
                        this.CurrentRemoteBinRoot));
                return false;
            }

            return true;
        }

        private void PatchBuildArtifact(BuildArtifact buildArtifact)
        {
            Debug.Assert(buildArtifact != null);

            // If this is ever made public, remember to start checking validity of BinRoots here.

            try
            {
                string localFilePath = Path.Combine(this.Repository.LocalBinRoot, buildArtifact.LocalPath);
                string stockRevisionFilePath = Path.Combine(this.CurrentRemoteBinRoot, buildArtifact.RemotePath + StockRevisionSuffix);
                string remoteFilePath = Path.Combine(this.CurrentRemoteBinRoot, buildArtifact.RemotePath);
                string emptyRevisionFilePath = stockRevisionFilePath + EmptyRevisionSuffix;

                // Stash a copy of the stock revision so that we can revert to it at will.
                if (!File.Exists(stockRevisionFilePath) && !File.Exists(emptyRevisionFilePath))
                {
                    if (File.Exists(remoteFilePath))
                    {
                        File.Move(remoteFilePath, stockRevisionFilePath);
                        File.SetAttributes(stockRevisionFilePath, FileAttributes.Hidden);
                    }
                    else
                    {
                        // If no file normally exists, create a special marker stock revision as an indicator
                        // that the file should be deleted on patch revert.
                        File.WriteAllText(emptyRevisionFilePath, string.Empty);
                        File.SetAttributes(emptyRevisionFilePath, FileAttributes.Hidden);
                    }
                }

                File.Copy(localFilePath, remoteFilePath, true);
            }
            catch (Exception ex)
            {
                // TODO: aggregate the exceptions into a single message box.
                this.ErrorDialogs.ErrorDialog(
                    string.Format(
                        Resources.StringPatchFailureGeneral,
                        buildArtifact.RemotePath,
                        ex.Message));
            }
        }

        private void RevertBuildArtifact(BuildArtifact buildArtifact)
        {
            Debug.Assert(buildArtifact != null);

            // If this is ever made public, remember to start checking validity of BinRoots here.

            try
            {
                string stockRevisionFilePath = Path.Combine(this.CurrentRemoteBinRoot, buildArtifact.RemotePath + StockRevisionSuffix);
                string remoteFilePath = Path.Combine(this.CurrentRemoteBinRoot, buildArtifact.RemotePath);
                string emptyRevisionFilePath = stockRevisionFilePath + EmptyRevisionSuffix;

                // Check if the patch added rather than replaced a file.
                if (File.Exists(emptyRevisionFilePath))
                {
                    File.Delete(remoteFilePath);
                    File.Delete(emptyRevisionFilePath);
                    return;
                }

                if (File.Exists(stockRevisionFilePath))
                {
                    if (File.Exists(remoteFilePath))
                    {
                        File.Delete(remoteFilePath);
                    }

                    File.Move(stockRevisionFilePath, remoteFilePath);
                }
            }
            catch (Exception ex)
            {
                // TODO: aggregate the exceptions into a single message box.
                this.ErrorDialogs.ErrorDialog(
                    string.Format(
                        Resources.StringPatchFailureGeneral,
                        buildArtifact.RemotePath,
                        ex.Message));
            }
        }
    }
}
