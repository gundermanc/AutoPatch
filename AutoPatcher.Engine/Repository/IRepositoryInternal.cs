using System.Collections.Generic;

namespace AutoPatcher.Engine.Repository
{
    internal interface IRepositoryInternal : IRepository
    {
        void AddBuildArtifactsRange(IEnumerable<BuildArtifact> buildArtifacts);

        void ClearBuildArtifacts();
    }
}
