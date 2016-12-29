using System;
using System.Windows.Input;
using AutoPatcher.Config;
using AutoPatcher.Models;
using AutoPatcher.Properties;
using AutoPatcher.Views;
using AutoPatcher.Abstractions;

namespace AutoPatcher.Commands
{
    internal sealed class AddBuildArtifactCommand : ICommand
    {
        private readonly IFileDialogs dialogs;
        private readonly PatchEditorModel model;

        public AddBuildArtifactCommand(IFileDialogs dialogs, PatchEditorModel model)
        {
            this.dialogs = dialogs;
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
            var model = new PathInputModel(
                this.dialogs,
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
