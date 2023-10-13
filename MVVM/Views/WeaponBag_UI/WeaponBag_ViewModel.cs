using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using MVVM;
using UI.MVVM.Models;
using UI.MVVM.Json_Asset;

public class WeaponBag_ViewModel : IViewModel
{
    public IModel I_model => _model;
    private Weapon_Model _model;

    public WeaponBag_ViewModel()
    {
        _model = GameManager.Ins.SourcesManager.WeaponModel;
        _model.M_PropertyChanged += Update_ViewModel_property;
        GetCharactorEWeaponLib = _model.GetCharactorEquipWeapons;
    }
    
    
    public event PropertyChangedEventHandler VM_PropertyChanged;
    public void Notify_VM_PropertyChanged(string propertyName)
    {
        VM_PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(propertyName));
    }

    public void Update_ViewModel_property(object sender, PropertyChangedEventArgs e)
    {
        
    }
    
    public Dictionary<string,A_Weapon>.Enumerator WeaponLibraryEnumerator => _model.WeaponAssetsEnumerator;

    public Func<CharactorType, ObservableCollection_AWeapon> GetCharactorEWeaponLib;

    public ObservableCollection_AWeapon CurrentCharactorEWeaponLib => _model.CurrentActiveCharactrorWeaponLib;
    ~WeaponBag_ViewModel()
     {
         _model.M_PropertyChanged -= Update_ViewModel_property;
     }
}
