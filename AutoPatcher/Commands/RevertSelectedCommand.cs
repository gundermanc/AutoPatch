using System.Linq;
using AutoPatcher.Abstractions;
using AutoPatcher.Engine.Repository;
using AutoPatcher.Models;

namespace AutoPatcher.Commands
{
    internal sealed class RevertSelectedCommand : RequiresRepoOpenCommandBase
    {
        private readonly IAbstraction abstraction;

        public RevertSelectedCommand(IAbstraction abstraction, MainWindowModel model) : base(model)
        {
            this.abstraction = abstraction;
        }

        public override void Execute(object parameter)
        {
            this.model.State.RevertBuildArtifacts(this.model.Selected.Cast<BuildArtifact>());
            this.model.RefreshBuildArtifacts();
        }
    }
}
