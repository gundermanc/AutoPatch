using AutoPatcher.Properties;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace AutoPatcher
{
    internal abstract class MainWindowModelBase : INotifyPropertyChanged
    {
        #region Privates 

        private string repoPath = null;
        private bool isBusy = false;
        private Cursor cursor = Cursors.Arrow;
        private string statusBarText = Resources.StringStatusBarReady;

        #endregion

        #region App Model Concepts

        public bool IsBusy
        {
            get
            {
                return this.isBusy;
            }

            set
            {
                if (value != this.isBusy)
                {
                    this.isBusy = value;
                    DispatchPropertyChanged(nameof(this.IsBusy));

                    this.Cursor = this.IsBusy ? Cursors.Wait : Cursors.Arrow;
                    this.StatusBarText = this.IsBusy ? Resources.StringStatusBarWorking : Resources.StringStatusBarReady;
                }
            }
        }

        public string RepoPath
        {
            get
            {
                return this.repoPath;
            }

            set
            {
                if (value != this.repoPath)
                {
                    this.repoPath = value;
                    DispatchPropertyChanged(nameof(this.RepoPath));
                }
            }
        }

        #endregion

        #region Window Properties and Events

        public event PropertyChangedEventHandler PropertyChanged;

        public Cursor Cursor
        {
            get
            {
                return this.cursor;
            }

            set
            {
                if (this.cursor != value)
                {
                    cursor = value;
                    DispatchPropertyChanged(nameof(this.Cursor));
                }
            }
        }

        public string StatusBarText
        {
            get
            {
                return this.statusBarText;
            }

            set
            {
                if (this.statusBarText != value)
                {
                    this.statusBarText = value;
                    DispatchPropertyChanged(nameof(this.StatusBarText));
                }
            }
        }

        #endregion

        #region Helpers

        protected void DispatchPropertyChanged(string propertyName)
        {
            Debug.Assert(propertyName != null);

            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
