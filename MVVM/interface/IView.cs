using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace MVVM
{
    public abstract class View<T>: MonoBehaviour where T : IViewModel
    {
        public abstract View<T> ParentView { get; }
        public abstract IViewModel ViewModel { get;}
        public abstract void RefreshView();
        
        public abstract void Update_View_Info(object sender, PropertyChangedEventArgs e);
    }

}
