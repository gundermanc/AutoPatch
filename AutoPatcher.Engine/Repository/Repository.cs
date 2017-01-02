using System.Collections.Generic;
using AutoPatcher.Engine.Util;

namespace AutoPatcher.Engine.Repository
{
    public sealed class Repository : IRepository
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
            this.BuildArtifacts = new HashSet<BuildArtifact>(buildArtifacts);
        }

        public string RepositoryConfigurationFilePath { get; }

        public string LocalBinRoot { get; set; }

        public string SourceItemsRoot { get; set; }

        public ISet<BuildArtifact> BuildArtifacts { get; }

        public void AddBuildArtifactsRange(IEnumerable<BuildArtifact> range)
        {
            foreach (var item in range)
            {
                this.BuildArtifacts.Add(item);
            }
        }
    }
}
