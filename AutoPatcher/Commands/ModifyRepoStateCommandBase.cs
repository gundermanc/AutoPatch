using System;
using System.Windows.Input;

namespace AutoPatcher.Commands
{
    internal abstract class ModifyRepoStateCommandBase : ICommand
    {
        protected readonly AppModel model;

        public ModifyRepoStateCommandBase(AppModel model)
        {
            this.model = model;
            this.model.PropertyChanged += Model_PropertyChanged;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return !this.model.IsModifyingLoadedAppConfiguration;
        }

        public abstract void Execute(object parameter);

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AppModel.IsModifyingLoadedAppConfiguration) && this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
