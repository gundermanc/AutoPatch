using System;
using System.Windows.Input;
using AutoPatcher.Models;

namespace AutoPatcher.Commands
{
    internal abstract class RequiresRepoOpenCommandBase : ICommand
    {
        protected readonly MainWindowModel model;

        public RequiresRepoOpenCommandBase(MainWindowModel model)
        {
            this.model = model;
            this.model.PropertyChanged += Model_PropertyChanged;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return this.model.State.Repository != null;
        }

        public abstract void Execute(object parameter);

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.model.RepoConfigPath) && this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
