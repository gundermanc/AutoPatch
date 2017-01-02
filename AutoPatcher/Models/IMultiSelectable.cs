using System;
using System.Collections.Generic;

namespace AutoPatcher.Models
{
    internal interface IMultiSelectable
    {
        event EventHandler ModelChangedSelection;

        IList<object> Selected { get; }

        void RaiseModelChangedSelectionEvent();
    }
}
