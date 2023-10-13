using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Equipments;
using DACore.DAWeaponData;
using NativeSerializableDictionary;
using UnityEngine.Serialization;

/// <summary>
/// 一把完整的枪类型（有枪械本体ID以及它身上装的配件信息）
/// </summary>
[System.Serializable]
public class A_Weapon
{
    public string hashID;
    public string nikeName; //玩家自定义名称
    public string name;
    public int level;
    [SerializeField] private DAWeaponSO baseSO;
    public DAWeaponSO BaseSO
    {
        get
        {
            if (baseSO == null && GameManager.Ins.SourcesManager.Weapon_SO.Library.TryGetValue(name, out DAWeaponSO so))
            { 
                baseSO = so; 
            }
            return baseSO;
        }
    }

    public int currentAmmoCount;
    
  
    /// <summary>
    /// 记录这把武器当前由谁装备（假如是单武器库，多角色共享的话还是有必要留的）
    /// </summary>

    public SerializableKVP<bool, CharactorType> holder;
    public A_Weapon(DAWeaponSO so, int? _level = null, int? _currentAmmoCount = null, string new_name = null, SerializableKVP<bool, CharactorType> holder_ = null)
    {
        hashID = Guid.NewGuid().ToString();//生成唯一ID
        baseSO = so;
        level = _level ?? 1;
        currentAmmoCount = _currentAmmoCount ?? so.ammoSO.ClipSize;
        nikeName = new_name ?? so.weaponName;
        name = so.weaponName;
        holder = holder_;
    }

    public void BeEuiped(CharactorType whoTake)
    {
        holder = new SerializableKVP<bool, CharactorType>(true, whoTake);
    }

    public void BeUnload()
    {
        holder = new SerializableKVP<bool, CharactorType>(false, CharactorType.None);
    }
}
