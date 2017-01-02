using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace AutoPatcher.Models
{
    internal class ModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Helpers

        protected void DispatchPropertyChanged(string propertyName)
        {
            Debug.Assert(propertyName != null);

            // Marshall all to the UI thread.
            Application.Current.Dispatcher.Invoke(
                () => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)));
        }

        #endregion
    }
}
