using System.Collections.Generic;

namespace AutoPatcher.Engine.Repository
{
    public interface IRepository
    {
        string RepositoryConfigurationFilePath { get; }

        string LocalBinRoot { get; set; }

        string SourceItemsRoot { get; set; }

        ISet<BuildArtifact> BuildArtifacts { get; }

        void AddBuildArtifactsRange(IEnumerable<BuildArtifact> range);
    }
}
