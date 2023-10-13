using MVVM;
using System.ComponentModel;
using DACore.DAWeaponData;
using UI.MVVM.Models;
using UnityEngine;
using UnityEngine.Assertions;

public class WeaponHUD_ViewModel : IViewModel
{
    public IModel I_model { get => _model; }
    private Weapon_Model _model;
    public WeaponHUD_ViewModel()
    {
        _model = GameManager.Ins.SourcesManager.WeaponModel;
        _model.M_PropertyChanged += Update_ViewModel_property;

        //VM层的私有字段变量与Model同步
        _mainWeapon = _model.MainWeapon;
        _deputyWeapon = _model.DeputyWeapon;
        
    }
 
    ~WeaponHUD_ViewModel()
    {
        _model.M_PropertyChanged -= Update_ViewModel_property;
    }
  

    public event PropertyChangedEventHandler VM_PropertyChanged;

    public void Notify_VM_PropertyChanged(string propertyName)
    {
        VM_PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void Update_ViewModel_property(object sender, PropertyChangedEventArgs e)
    {
        if(e.PropertyName==nameof(_model.MainWeapon))
        {
            MainWeapon = _model.MainWeapon;
        }
        else if(e.PropertyName == nameof(_model.DeputyWeapon))
        {
            DeputyWeapon = _model.DeputyWeapon; 
        }
        else if(e.PropertyName == nameof(_model.ActiveWeapon))//切枪/捡起地上的临时武器也刷新弹药HUD
        {
            Notify_VM_PropertyChanged(nameof(ActiveWeapon));
        }
        else if (e.PropertyName == nameof(_model.CurrentClipAmmo))
        {
            Notify_VM_PropertyChanged(nameof(CurrentWeaponAmmo));
        }
        else if(e.PropertyName == nameof(_model.ReserveAmmo))
        {
            Notify_VM_PropertyChanged(nameof(CurrentReserveTypeAmmo));
        }
        else if (e.PropertyName == nameof(_model.CurrentActiveCharactrorWeaponLib))
        {
            MainWeapon = _model.MainWeapon;
            DeputyWeapon = _model.DeputyWeapon; 
            Notify_VM_PropertyChanged(nameof(ActiveWeapon));
        }
    }

    #region 主武器信息
    private A_Weapon _mainWeapon;
    public A_Weapon MainWeapon
    {
        get => _mainWeapon;
        set
        {
            if (_mainWeapon != value)
            {
                if (_mainWeapon == null || value == null)
                {
                    _mainWeapon = value;
                    Notify_VM_PropertyChanged(nameof(MainWeaponSprite));
                    Notify_VM_PropertyChanged(nameof(MainWeaponType));
                    Notify_VM_PropertyChanged(nameof(MainWeaponName));
                }
                else
                {
                    if (_mainWeapon.BaseSO.UI_Sources.vertical_Icon != value.BaseSO.UI_Sources.vertical_Icon)
                        Notify_VM_PropertyChanged(nameof(MainWeaponSprite));
                    if (_mainWeapon.BaseSO.WeaponUIType != value.BaseSO.WeaponUIType)
                        Notify_VM_PropertyChanged(nameof(MainWeaponType));
                    if (_mainWeapon.name != value.name)
                        Notify_VM_PropertyChanged(nameof(MainWeaponName));
                    _mainWeapon = value;
                }
            }
        }
    }

    public Sprite MainWeaponSprite
    {
        get
        {
            if (MainWeapon == null) return null;
            return MainWeapon.BaseSO.UI_Sources.vertical_Icon;
        }
    }
    public string MainWeaponType { get => MainWeapon == null ? string.Empty : MainWeapon.BaseSO.WeaponUIType.ToString(); }

    public string MainWeaponName { get => MainWeapon == null ? string.Empty : MainWeapon.name; }

    #endregion

    #region 副武器信息
    private A_Weapon _deputyWeapon;
    public A_Weapon DeputyWeapon
    {
        get
        {
            return _deputyWeapon;
        }
        set
        {
            if (_deputyWeapon != value)
            {
                if (_deputyWeapon == null || value == null)
                {
                    _deputyWeapon = value;
                    Notify_VM_PropertyChanged(nameof(DeputyWeaponSprite));
                    Notify_VM_PropertyChanged(nameof(DeputyWeaponType));
                    Notify_VM_PropertyChanged(nameof(DeputyWeaponName));
                }
                else
                {
                    if (_deputyWeapon.BaseSO.UI_Sources.vertical_Icon != value.BaseSO.UI_Sources.vertical_Icon)
                        Notify_VM_PropertyChanged(nameof(DeputyWeaponSprite));
                    if (_deputyWeapon.BaseSO.WeaponUIType != value.BaseSO.WeaponUIType)
                        Notify_VM_PropertyChanged(nameof(DeputyWeaponType));
                    if (_deputyWeapon.name != value.name)
                        Notify_VM_PropertyChanged(nameof(DeputyWeaponName));
                    _deputyWeapon = value;
                }
                
            }
        }
    }

    public Sprite DeputyWeaponSprite
    {
        get
        {
            if (DeputyWeapon == null) return null;
            return DeputyWeapon.BaseSO.UI_Sources.vertical_Icon;
        }
    }
    public string DeputyWeaponType { get => DeputyWeapon == null ? string.Empty : DeputyWeapon.BaseSO.WeaponUIType.ToString(); }

    public string DeputyWeaponName { get => DeputyWeapon == null ? string.Empty : DeputyWeapon.name; }

    #endregion
    
    public A_Weapon ActiveWeapon => _model.ActiveWeapon;

    #region 当前武器的弹药信息
    public int CurrentWeaponAmmo
    {
        get => _model.CurrentClipAmmo;
    }
    
    public int CurrentReserveTypeAmmo
    {
        get => _model.ReserveAmmo;
    }
    #endregion
    
    
    public void SelfSave()
    {
        _model.SelfSave();
    }
}
