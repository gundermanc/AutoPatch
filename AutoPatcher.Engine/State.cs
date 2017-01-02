using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

        private readonly IDictionary<string, BuildArtifact> sourceItemPathToBuildArtifactLookupTable = new Dictionary<string, BuildArtifact>();

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

        public IRepository Repository => this.MutableRepository;

        private IRepositoryInternal MutableRepository { get; set; }

        private IErrorDialogs ErrorDialogs { get; }

        private IFileDialogs FileDialogs { get; }

        private ISettingsManager SettingsManager { get; }

        public async Task CreateAndLoadRepositoryAsync(string filePath)
        {
            UnloadRepository();

            this.MutableRepository = RepositoryLoader.CreateEmptyFile(this.ErrorDialogs, filePath);

            await SaveRepositoryAsync();
        }

        public async Task LoadRepositoryAsync(string filePath)
        {
            this.UnloadRepository();
            this.MutableRepository = await Task.Run(() => RepositoryLoader.CreateFromFile(this.ErrorDialogs, filePath));

            this.RefreshBuildArtifactStates(this.Repository.BuildArtifacts);
            this.AddBuildArtifactsRangeToLookupTable(this.Repository.BuildArtifacts);
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
                this.MutableRepository = null;
                this.sourceItemPathToBuildArtifactLookupTable.Clear();
            }
        }

        public void PatchBuildArtifacts(IEnumerable<BuildArtifact> buildArtifacts)
        {
            Verify.IsNotNull(buildArtifacts, nameof(buildArtifacts));

            if (!this.IsInActionableState())
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

            if (!this.IsInActionableState(true))
            {
                return;
            }

            foreach (var artifact in buildArtifacts)
            {
                RevertBuildArtifact(artifact);
            }
        }

        public void ClearBuildArtifacts()
        {
            this.MutableRepository.ClearBuildArtifacts();
            this.sourceItemPathToBuildArtifactLookupTable.Clear();
        }

        public void AddBuildArtifactsRange(IEnumerable<BuildArtifact> buildArtifacts)
        {
            Verify.IsNotNull(buildArtifacts, nameof(buildArtifacts));

            this.RefreshBuildArtifactStates(buildArtifacts);
            this.MutableRepository.AddBuildArtifactsRange(buildArtifacts);
            this.AddBuildArtifactsRangeToLookupTable(buildArtifacts);
        }

        public void RefreshBuildArtifactStates()
        {
            // TODO: should this be async?
            if (this.Repository == null)
            {
                throw new InvalidOperationException("No repository loaded");
            }

            this.RefreshBuildArtifactStates(this.Repository.BuildArtifacts);
        }

        public async Task<IEnumerable<BuildArtifact>> GetDirtyBuildArtifactsAsync() => await Task.Run(() => GetDirtyBuildArtifacts());

        private IEnumerable<BuildArtifact> GetDirtyBuildArtifacts()
        {
            if (!this.IsInActionableState(isSourceOp: true))
            {
                yield break;
            }

            IEnumerable<string> dirtySourceItemPaths = null;

            try
            {
                dirtySourceItemPaths = this.GetDirtySourceItemPaths();
            }
            catch (Exception ex)
            {
                this.ErrorDialogs.ErrorDialog(
                    string.Format(
                        Resources.StringFailureGeneral,
                        ex.Message));

                yield break;
            }

            foreach (var path in dirtySourceItemPaths)
            {
                BuildArtifact artifact;

                if (this.sourceItemPathToBuildArtifactLookupTable.TryGetValue(path, out artifact))
                {
                    yield return artifact;
                }
                else
                {
                    // TODO: aggregate all into a single dialog.
                    this.ErrorDialogs.WarningDialog(
                        string.Format(
                            Resources.StringUnmappedFile,
                            path));
                }
            }
        }

        private IEnumerable<string> GetDirtySourceItemPaths()
        {
            Debug.Assert(!string.IsNullOrEmpty(this.Repository.SourceItemsRoot));

            var sourceItemPaths = new List<string>();

            try
            {
                using (var repo = new LibGit2Sharp.Repository(this.Repository.SourceItemsRoot))
                {
                    // TODO: get new files as dirty items.
                    foreach (var change in repo.Diff.Compare<LibGit2Sharp.TreeChanges>())
                    {
                        sourceItemPaths.Add(change.Path);

                        if (change.OldPath != change.Path)
                        {
                            sourceItemPaths.Add(change.OldPath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.ErrorDialogs.ErrorDialog(
                    string.Format(
                        Resources.StringFailureGeneral,
                        ex.Message));
            }

            return sourceItemPaths;
        }

        private void RefreshBuildArtifactStates(IEnumerable<BuildArtifact> buildArtifacts)
        {
            foreach (var buildArtifact in buildArtifacts)
            {
                string stockRevisionFilePath = Path.Combine(this.CurrentRemoteBinRoot, buildArtifact.RemotePath + StockRevisionSuffix);
                string emptyRevisionFilePath = stockRevisionFilePath + EmptyRevisionSuffix;

                if (File.Exists(stockRevisionFilePath) || File.Exists(emptyRevisionFilePath))
                {
                    buildArtifact.IsPatched = true;
                }
                else
                {
                    buildArtifact.IsPatched = false;
                }
            }
        }

        private void AddBuildArtifactsRangeToLookupTable(IEnumerable<BuildArtifact> buildArtifacts)
        {
            // TODO: analyze perf impact.
            foreach (var buildArtifact in this.Repository.BuildArtifacts)
            {
                foreach (var sourceItem in buildArtifact.SourceItems)
                {
                    this.sourceItemPathToBuildArtifactLookupTable.Add(sourceItem.LocalPath, buildArtifact);
                }
            }
        }

        private bool IsInActionableState(bool isRevert = false, bool isSourceOp = false)
        {
            if (this.Repository == null)
            {
                throw new InvalidOperationException("No repository loaded");
            }

            if (!isRevert && !Directory.Exists(this.Repository.LocalBinRoot))
            {
                this.ErrorDialogs.ErrorDialog(
                    string.Format(
                        Resources.StringFailureInvalidLocalBinRoot,
                        this.CurrentRemoteBinRoot));
                return false;
            }

            if (!Directory.Exists(this.CurrentRemoteBinRoot))
            {
                this.ErrorDialogs.ErrorDialog(
                    string.Format(
                        Resources.StringFailureInvalidRemoteBinRoot,
                        this.CurrentRemoteBinRoot));
                return false;
            }

            if (isSourceOp && !Directory.Exists(this.Repository.SourceItemsRoot))
            {
                this.ErrorDialogs.ErrorDialog(
                    string.Format(
                        Resources.StringFailureInvalidSourceItemsRoot,
                        this.Repository.SourceItemsRoot));
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
                buildArtifact.IsPatched = true;
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
                    buildArtifact.IsPatched = false;
                    return;
                }

                if (File.Exists(stockRevisionFilePath))
                {
                    if (File.Exists(remoteFilePath))
                    {
                        File.Delete(remoteFilePath);
                    }

                    File.Move(stockRevisionFilePath, remoteFilePath);
                    buildArtifact.IsPatched = false;
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
