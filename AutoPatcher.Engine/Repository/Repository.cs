using System.Collections.Generic;
using AutoPatcher.Engine.Util;

namespace AutoPatcher.Engine.Repository
{
    public sealed class Repository : IRepositoryInternal
    {
        public const string ConfigurationFileFilter = "AutoPatcher Configuration|*.apconfig";

        public Repository(
            string repositoryConfigurationFilePath,
            string localBinRoot,
            string sourceItemsRoot,
            IEnumerable<BuildArtifact> buildArtifacts)
        {
            Verify.IsNotNullOrWhiteSpace(repositoryConfigurationFilePath, nameof(repositoryConfigurationFilePath));
            Verify.IsNotNull(buildArtifacts, nameof(buildArtifacts));

            this.RepositoryConfigurationFilePath = repositoryConfigurationFilePath;
            this.LocalBinRoot = localBinRoot;
            this.SourceItemsRoot = sourceItemsRoot;
            this.MutableBuildArtifacts = new List<BuildArtifact>(buildArtifacts);
        }

        #region IRepository Members

        public string RepositoryConfigurationFilePath { get; }

        public string LocalBinRoot { get; set; }

        public string SourceItemsRoot { get; set; }

        public IReadOnlyList<BuildArtifact> BuildArtifacts => this.MutableBuildArtifacts;

        #endregion

        #region IRepository Internal Members

        internal List<BuildArtifact> MutableBuildArtifacts { get; }

        public void AddBuildArtifactsRange(IEnumerable<BuildArtifact> buildArtifacts)
            => this.MutableBuildArtifacts.AddRange(buildArtifacts);

        public void ClearBuildArtifacts() => this.MutableBuildArtifacts.Clear();

        #endregion
    }
}
