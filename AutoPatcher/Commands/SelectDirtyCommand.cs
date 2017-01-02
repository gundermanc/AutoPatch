using AutoPatcher.Abstractions;
using AutoPatcher.Models;

namespace AutoPatcher.Commands
{
    internal sealed class SelectDirtyCommand : RequiresRepoOpenCommandBase
    {
        private readonly IAbstraction abstraction;

        public SelectDirtyCommand(IAbstraction abstraction, MainWindowModel model) : base(model)
        {
            this.abstraction = abstraction;
        }

        public async override void Execute(object parameter)
        {
            this.model.Selected.Clear();

            foreach (var buildArtifact in await this.model.State.GetDirtyBuildArtifactsAsync())
            {
                this.model.Selected.Add(buildArtifact);
            }

            this.model.RaiseModelChangedSelectionEvent();
        }
    }
}
