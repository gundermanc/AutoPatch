using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace AutoPatcher
{
    internal abstract class MainWindowModelBase : INotifyPropertyChanged
    {
        #region Privates 

        private bool isBusy = false;
        private Cursor cursor = Cursors.Arrow;

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
                }

                this.Cursor = this.IsBusy ? Cursors.Wait : Cursors.Arrow;
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

        public abstract string Title { get; }

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
