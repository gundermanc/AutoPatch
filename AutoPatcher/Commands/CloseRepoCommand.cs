using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AutoPatcher.Commands
{
    internal sealed class CloseRepoCommand : ICommand
    {
        private readonly AppModel model;

        public CloseRepoCommand(AppModel model)
        {
            this.model = model;
            this.model.PropertyChanged += Model_PropertyChanged;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return this.model.RepoPath != null;
        }

        public void Execute(object parameter)
        {
            Task.Run(() => this.model.UnloadAppConfigurationAsync());
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.model.RepoPath) && this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
