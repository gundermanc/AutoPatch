using System.Threading.Tasks;
using AutoPatcher.Abstractions;
using AutoPatcher.Engine.ArtifactLocator;
using AutoPatcher.Engine.MSBuild;
using AutoPatcher.Models;
using AutoPatcher.Properties;

namespace AutoPatcher.Commands
{
    internal sealed class ImportFromMSBuildCommand : RequiresRepoOpenCommandBase
    {
        private readonly IAbstraction abstraction;

        public ImportFromMSBuildCommand(IAbstraction abstraction, MainWindowModel model) : base(model)
        {
            this.abstraction = abstraction;
        }

        public async override void Execute(object parameter)
        {
            var filePath = this.abstraction.FileDialogs.OpenFileDialog(
                Resources.StringImportMSBuildTitle,
                MSBuildArtifactImporter.MSBuildFileFilter,
                null,
                true);

            if (filePath != null)
            {
                this.model.IsBusy = true;

                await Task.Run(() => MSBuildArtifactImporter.Import(
                    errorDialog: this.model.Abstraction.ErrorDialogs,
                    state: this.model.State,
                    localBinLocator: new FileEnumArtifactLocator(
                        this.model.State.Repository.LocalBinRoot,
                        new ExeArtifactTypeMatcher(),
                        new DllArtifactTypeMatcher()),
                    remoteBinLocator: new FileEnumArtifactLocator(
                        this.model.State.CurrentRemoteBinRoot,
                        new ExeArtifactTypeMatcher(),
                        new DllArtifactTypeMatcher()),
                    fileName: filePath));

                this.model.SaveRepository();
                this.model.DispatchRepositoryPropertiesChanged();
                this.model.RefreshBuildArtifacts();

                this.model.IsBusy = false;
            }
        }
    }
}
