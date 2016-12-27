using System.Windows.Input;
using AutoPatcher.Properties;

namespace AutoPatcher.Models
{
    internal abstract class MainWindowModelBase : ModelBase
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

        public string RepoConfigPath
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
                    DispatchPropertyChanged(nameof(this.RepoConfigPath));
                }
            }
        }

        #endregion

        #region Window Properties and Events

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
    }
}
