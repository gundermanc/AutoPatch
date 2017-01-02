using AutoPatcher.Abstractions;
using AutoPatcher.Engine.Util;
using AutoPatcher.Models;
using AutoPatcher.Views;

namespace AutoPatcher.Commands
{
    internal sealed class EditPatchSchemeCommand : RequiresRepoOpenCommandBase
    {
        private readonly IAbstraction abstraction;

        public EditPatchSchemeCommand(IAbstraction abstraction, MainWindowModel model) : base(model)
        {
            this.abstraction = abstraction;
        }

        public override void Execute(object parameter)
        {
            var model = new PatchEditorModel(
                this.abstraction,
                this.model.State,
                this.model.State.Repository.BuildArtifacts.Clone());

            if (new PatchEditorWindow() { DataContext = model }.ShowDialog() ?? false)
            {
                this.model.State.ClearBuildArtifacts();
                this.model.State.AddBuildArtifactsRange(model.BuildArtifacts);
                this.model.SaveRepository();
                this.model.DispatchRepositoryPropertiesChanged();
                this.model.RefreshBuildArtifacts();
            }
        }
    }
}
