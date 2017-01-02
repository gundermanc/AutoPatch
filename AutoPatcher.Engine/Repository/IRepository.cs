using System.Collections.Generic;

namespace AutoPatcher.Engine.Repository
{
    public interface IRepository
    {
        string RepositoryConfigurationFilePath { get; }

        string LocalBinRoot { get; set; }

        string SourceItemsRoot { get; set; }

        IReadOnlyList<BuildArtifact> BuildArtifacts { get; }
    }
}
