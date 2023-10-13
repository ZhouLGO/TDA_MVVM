using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVVM;
using System.ComponentModel;

public class Fitting_Model : IModel
{
    public event PropertyChangedEventHandler M_PropertyChanged;

    public void NotifyModelPropertyChanged(string propertyName)
    {
        throw new System.NotImplementedException();
    }
}
