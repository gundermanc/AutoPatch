using System;
using System.Linq;
using System.Windows.Input;
using AutoPatcher.Abstractions;
using AutoPatcher.Engine.Repository;
using AutoPatcher.Models;
using AutoPatcher.Properties;
using AutoPatcher.Views;

namespace AutoPatcher.Commands
{
    internal sealed class AddBuildArtifactCommand : ICommand
    {
        private readonly IAbstraction abstraction;
        private readonly PatchEditorModel model;

        public AddBuildArtifactCommand(IAbstraction abstraction, PatchEditorModel model)
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
            var model = new PathInputModel(
                this.abstraction,
                Resources.StringAddBuildArtifactTitle,
                Resources.StringLocalPathContent,
                Resources.StringRemotePathContent,
                this.model.State.Repository.LocalBinRoot,
                this.model.State.CurrentRemoteBinRoot,
                input0EnsureExists: false,
                input1EnsureExists: false);

            if (new PathInputWindow() { DataContext = model }.ShowDialog() ?? false)
            {
                this.model.BuildArtifacts.Add(
                    new BuildArtifact(
                        model.Input0Text,
                        model.Input1Text,
                        Enumerable.Empty<SourceItem>()));
            }
        }
    }
}
