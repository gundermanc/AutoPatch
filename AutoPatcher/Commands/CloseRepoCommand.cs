﻿using System;
using System.Windows.Input;
using AutoPatcher.Models;

namespace AutoPatcher.Commands
{
    internal sealed class CloseRepoCommand : ICommand
    {
        private readonly MainWindowModel model;

        public CloseRepoCommand(MainWindowModel model)
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
            this.model.UnloadRepository();
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
