using AutoPatcher.Engine.Repository;
using AutoPatcher.Models;
using AutoPatcher.Properties;

namespace AutoPatcher.Commands
{
    internal sealed class NewRepoCommand : ModifyRepoStateCommandBase
    {
        public NewRepoCommand(MainWindowModel model) : base(model)
        {
        }

        public override void Execute(object parameter)
        {
            var configPath = model.Abstraction.FileDialogs.NewFileDialog(
                Resources.StringNewRepoDialogTitle,
                Repository.ConfigurationFileFilter,
                null);

            if (!string.IsNullOrWhiteSpace(configPath))
            {
                this.model.CreateAndLoadRepository(configPath);
            }
        }
    }
}
