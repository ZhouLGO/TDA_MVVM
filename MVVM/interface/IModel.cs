using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
namespace MVVM
{
    public interface IModel
    {  
        event PropertyChangedEventHandler M_PropertyChanged;
        void NotifyModelPropertyChanged(string propertyName);
    }
}