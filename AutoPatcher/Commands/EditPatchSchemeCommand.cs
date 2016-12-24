using System;
using System.Windows.Input;

namespace AutoPatcher.Commands
{
    internal sealed class EditPatchSchemeCommand : ICommand
    {
        private readonly AppModel model;

        public EditPatchSchemeCommand(AppModel model)
        {
            this.model = model;
            this.model.PropertyChanged += Model_PropertyChanged;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return this.model.RepoConfigPath != null;
        }

        public void Execute(object parameter)
        {
            new PatchEditorWindow()
            {
                DataContext = new PatchEditorModel()
            }.ShowDialog();
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.model.RepoConfigPath) && this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
