using AutoPatcher.Models;

namespace AutoPatcher.Commands
{
    internal sealed class CloseRepoCommand : RequiresRepoOpenCommandBase
    {
        public CloseRepoCommand(MainWindowModel model) : base (model) { }

        public override void Execute(object parameter)
        {
            this.model.UnloadRepository();
        }
    }
}
