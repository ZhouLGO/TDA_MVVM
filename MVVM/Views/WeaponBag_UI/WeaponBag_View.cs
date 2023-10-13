using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using MVVM;
using UI.MVVM.Json_Asset;

public class WeaponBag_View : View<WeaponBag_ViewModel>
{
    public override View<WeaponBag_ViewModel> ParentView => null;
    public override IViewModel ViewModel => _viewModel;
    private WeaponBag_ViewModel _viewModel;
    [SerializeField] private GameObject weaponSoltBarPrefab;
    [SerializeField] private List<WeaponSlotBar_View> BarEntitys = new List<WeaponSlotBar_View>();
    public Transform WeaponBarContainer;
   
     private void Awake()
    {
        _viewModel = new WeaponBag_ViewModel();
        UIManager.Instance.WeaponBagView = this;
    }

     private void OnEnable()
     {
         _viewModel.VM_PropertyChanged += Update_View_Info;
         RefreshView();
     }
     
     //bar后面应该用对象池装
     private void OnDisable()
     {
         for (int i = BarEntitys.Count - 1; i >= 0; i--)
         {
             var target = BarEntitys[i];
             BarEntitys.Remove(target);
             Destroy(target.gameObject);
         }
         _viewModel.VM_PropertyChanged -= Update_View_Info;
     }
     
     
     private bool _selected;
     private WeaponSlotBar_View _currentSelectedWeapon;
     private void OnGUI()
     {
         if (_selected)
         {
             //装备按钮
             if (_currentSelectedWeapon.Weapon.holder.Key == false)
             {
                 // 获取当前角色的武器装备字典
                 if (_viewModel.CurrentCharactorEWeaponLib.Count < 2)
                 {
                     // 创建按钮，并检查是否被点击
                     if (GUI.Button(new Rect(transform.position.x, transform.position.y, 100, 30), "装备"))
                     {
                         _viewModel.CurrentCharactorEWeaponLib.Add(_currentSelectedWeapon.Weapon);
                         _currentSelectedWeapon.RefreshSlotInfo();
                     }
                 }
             }

             //卸下按钮
             if (_currentSelectedWeapon.Weapon.holder.Key == true)
             {
                 // 获取对象武器装备字典
                 var whoWeaponLib = _viewModel.GetCharactorEWeaponLib(_currentSelectedWeapon.Weapon.holder.Value);
                 if (whoWeaponLib.Contains(_currentSelectedWeapon.Weapon))
                 {
                     if (GUI.Button(new Rect(transform.position.x, transform.position.y - 50, 100, 30), "卸下"))
                     {
                         // 在这里执行卸下的逻辑
                         whoWeaponLib.Remove(_currentSelectedWeapon.Weapon);
                         _currentSelectedWeapon.RefreshSlotInfo();
                     }
                 }
             }
         }
     }
     

     public override void RefreshView()
    {
        using (var enumerator = _viewModel.WeaponLibraryEnumerator)
        {
            while (enumerator.MoveNext())
            {
                var weaponKVP = enumerator.Current;
                if(BarEntitys.Any(bar => bar.Weapon.hashID == weaponKVP.Value.hashID)) continue;
                
                GameObject slotPrefab = Instantiate(weaponSoltBarPrefab, WeaponBarContainer, false);
                WeaponSlotBar_View slotBar = slotPrefab.GetComponent<WeaponSlotBar_View>();
                BarEntitys.Add(slotBar);
                slotBar.OnBarClicked += (sender) =>
                {
                    _currentSelectedWeapon = sender;
                    _selected = true;
                };
                slotBar.Weapon = weaponKVP.Value;
            }
        }
    }

    public override void Update_View_Info(object sender, PropertyChangedEventArgs e)
    {
        
    }
}
