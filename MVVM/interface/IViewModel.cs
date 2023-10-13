using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace MVVM
{
    public interface IViewModel
    {
        public IModel I_model { get; }
        event PropertyChangedEventHandler VM_PropertyChanged;
        void Notify_VM_PropertyChanged(string propertyName);
        void Update_ViewModel_property(object sender, PropertyChangedEventArgs e);
    }
}
