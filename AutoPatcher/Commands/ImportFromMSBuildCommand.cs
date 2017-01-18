using System;
using System.Windows.Input;
using AutoPatcher.Abstractions;
using AutoPatcher.Engine.ArtifactLocator;
using AutoPatcher.Engine.MSBuild;
using AutoPatcher.Models;
using AutoPatcher.Properties;

namespace AutoPatcher.Commands
{
    internal sealed class ImportFromMSBuildCommand : ICommand
    {
        private readonly IAbstraction abstraction;
        private readonly MainWindowModel model;

        public ImportFromMSBuildCommand(IAbstraction abstraction, MainWindowModel model)
        {
            this.abstraction = abstraction;
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
            var filePath = this.abstraction.FileDialogs.OpenFileDialog(
                Resources.StringImportMSBuildTitle,
                MSBuildArtifactImporter.MSBuildFileFilter,
                null,
                true);

            if (filePath != null)
            {
                this.model.IsBusy = true;

                MSBuildArtifactImporter.Import(
                    errorDialog: this.model.Abstraction.ErrorDialogs,
                    state: this.model.State,
                    localBinLocator: new FileEnumArtifactLocator(
                        this.model.State.Repository.LocalBinRoot,
                        new ExeArtifactTypeMatcher(),
                        new DllArtifactTypeMatcher()),
                    remoteBinLocator: new FileEnumArtifactLocator(
                        this.model.State.CurrentRemoteBinRoot,
                        new ExeArtifactTypeMatcher(),
                        new DllArtifactTypeMatcher()),
                    fileName: filePath);

                this.model.IsBusy = false;
            }
        }
    }
}
