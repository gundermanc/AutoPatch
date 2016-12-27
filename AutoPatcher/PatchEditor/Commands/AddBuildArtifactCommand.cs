using AutoPatcher.Config;
using AutoPatcher.Properties;
using System;
using System.Windows.Input;

namespace AutoPatcher.PatchEditor.Commands
{
    internal sealed class AddBuildArtifactCommand : ICommand
    {
        private PatchEditorModel model;

        public AddBuildArtifactCommand(PatchEditorModel model)
        {
            this.model = model;
        }

#pragma warning disable 0067
        public event EventHandler CanExecuteChanged;
#pragma warning restore 0067

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var model = new InputModel(
                Resources.StringAddBuildArtifactTitle,
                Resources.StringLocalPathContent,
                Resources.StringRemotePathContent);

            var result = new InputWindow()
            {
                DataContext = model
            }.ShowDialog();

            if (result ?? false)
            {
                this.model.BuildArtifacts.Add(
                    new BuildArtifactData()
                    {
                        LocalPath = model.Input0Text,
                        RemotePath = model.Input1Text
                    });
            }
        }
    }
}
