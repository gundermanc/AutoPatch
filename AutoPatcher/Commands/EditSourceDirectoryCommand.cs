using AutoPatcher.Abstractions;
using AutoPatcher.Models;
using AutoPatcher.Properties;
using AutoPatcher.Views;

namespace AutoPatcher.Commands
{
    internal sealed class EditSourceDirectoryCommand : RequiresRepoOpenCommandBase
    {
        private readonly IAbstraction abstraction;

        public EditSourceDirectoryCommand(
            IAbstraction abstraction,
            MainWindowModel model) : base(model)
        {
            this.abstraction = abstraction;
        }
        
        public override void Execute(object parameter)
        {
            var model = new PathInputModel(
                this.abstraction,
                Resources.StringEditSourcesDirectoryDialogTitle,
                Resources.StringLocalSourcesDirectoryContent,
                openFolderInsteadOfFile: true)
            {
                Input0Text = this.model.State.Repository.SourceItemsRoot
            };

            if (new PathInputWindow() { DataContext = model }.ShowDialog() ?? false)
            {
                this.model.State.Repository.SourceItemsRoot = model.Input0Text;

                this.model.SaveRepository();
            }
        }
    }
}
