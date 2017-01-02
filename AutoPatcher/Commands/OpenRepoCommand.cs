using AutoPatcher.Engine.Repository;
using AutoPatcher.Models;
using AutoPatcher.Properties;

namespace AutoPatcher.Commands
{
    internal sealed class OpenRepoCommand : ModifyRepoStateCommandBase
    {
        public OpenRepoCommand(MainWindowModel model) : base(model)
        {
        }

        public override void Execute(object parameter)
        {
            var repoPath = model.Abstraction.FileDialogs.OpenFileDialog(
                Resources.StringOpenRepoDialogTitle,
                Repository.ConfigurationFileFilter,
                null,
                true);

            if (repoPath != null)
            {
                model.LoadRepository(repoPath);
            }
        }
    }
}
