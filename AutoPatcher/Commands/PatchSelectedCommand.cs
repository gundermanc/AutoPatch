using System;
using System.IO;
using System.Windows.Input;
using AutoPatcher.Abstractions;
using AutoPatcher.Models;
using AutoPatcher.Properties;

namespace AutoPatcher.Commands
{
    internal sealed class PatchSelectedCommand : ICommand
    {
        private readonly IErrorDialogs dialogs;
        private readonly MainWindowModel model;

        public PatchSelectedCommand(IErrorDialogs dialogs, MainWindowModel model)
        {
            this.dialogs = dialogs;
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
            foreach (var file in this.model.SelectedBuildArtifacts)
            {
                Exception ioException = null;

                try
                {
                    string remoteFilePath = Path.Combine(this.model.RemoteBinRoot, file.RemotePath);
                    string localFilePath = Path.Combine(this.model.LocalBinRoot, file.LocalPath);

                    if (File.Exists(remoteFilePath))
                    {
                        string revisionRemoteFileName = remoteFilePath + ".old.ticks-" + DateTime.Now.Ticks;
                        File.Move(remoteFilePath, revisionRemoteFileName);
                        File.SetAttributes(revisionRemoteFileName, FileAttributes.Hidden);
                    }

                    File.Copy(localFilePath, remoteFilePath);
                }
                catch (ArgumentException ex)
                {
                    ioException = ex;
                }
                catch (IOException ex)
                {
                    ioException = ex;
                }
                catch (UnauthorizedAccessException ex)
                {
                    ioException = ex;
                }

                if (ioException != null)
                {
                    dialogs.ErrorDialog(
                        string.Format(
                            Resources.StringFilePatchFailure,
                            file.RemotePath,
                            ioException.Message));
                }
            }
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
