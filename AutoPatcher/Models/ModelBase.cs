using System.ComponentModel;
using System.Diagnostics;

namespace AutoPatcher.Models
{
    internal class ModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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
