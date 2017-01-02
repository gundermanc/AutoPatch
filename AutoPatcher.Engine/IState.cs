using System.Collections.Generic;
using System.Threading.Tasks;
using AutoPatcher.Engine.Repository;


namespace AutoPatcher.Engine
{
    public interface IState
    {
        string CurrentRemoteBinRoot { get; set; }

        IRepository Repository { get; }

        Task CreateAndLoadRepositoryAsync(string filePath);

        Task LoadRepositoryAsync(string filePath);

        Task SaveRepositoryAsync();

        void UnloadRepository();

        void PatchBuildArtifacts(IEnumerable<BuildArtifact> buildArtifacts);

        void RevertBuildArtifacts(IEnumerable<BuildArtifact> buildArtifacts);

        void ClearBuildArtifacts();

        void AddBuildArtifactsRange(IEnumerable<BuildArtifact> buildArtifact);

        void RefreshBuildArtifactStates();
    }
}
