using System.Linq;
using AutoPatcher.Abstractions;
using AutoPatcher.Engine.Repository;
using AutoPatcher.Models;

namespace AutoPatcher.Commands
{
    internal sealed class PatchSelectedCommand : RequiresRepoOpenCommandBase
    {
        private readonly IAbstraction abstraction;

        public PatchSelectedCommand(IAbstraction abstraction, MainWindowModel model) : base(model)
        {
            this.abstraction = abstraction;
        }

        public override void Execute(object parameter)
        {
            this.model.State.PatchBuildArtifacts(this.model.Selected.Cast<BuildArtifact>());
            this.model.RefreshBuildArtifacts();
        }
    }
}
