using AutoPatcher.Abstractions;
using AutoPatcher.Models;
using AutoPatcher.Properties;
using AutoPatcher.Views;

namespace AutoPatcher.Commands
{
    internal sealed class EditBinaryDirectoriesCommand : RequiresRepoOpenCommandBase
    {
        private readonly IAbstraction abstraction;

        public EditBinaryDirectoriesCommand(
            IAbstraction abstraction,
            MainWindowModel model) : base(model)
        {
            this.abstraction = abstraction;
        }

        public override void Execute(object parameter)
        {
            var model = new PathInputModel(
                this.abstraction,
                Resources.StringEditBinaryDirectoriesDialogTitle,
                Resources.StringLocalBinariesDirectoryContent,
                Resources.StringRemoteBinariesDirectoryContent,
                openFolderInsteadOfFile: true)
            {
                Input0Text = this.model.State.Repository.LocalBinRoot,
                Input1Text = this.model.State.CurrentRemoteBinRoot
            };

            if (new PathInputWindow() { DataContext = model }.ShowDialog() ?? false)
            {
                this.model.State.Repository.LocalBinRoot = model.Input0Text;
                this.model.State.CurrentRemoteBinRoot = model.Input1Text;

                this.model.State.RefreshBuildArtifactStates();
                this.model.RefreshBuildArtifacts();

                this.model.SaveRepository();
            }
        }
    }
}
