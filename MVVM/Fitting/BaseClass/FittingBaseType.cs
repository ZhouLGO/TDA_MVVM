using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeSerializableDictionary;

namespace Equipments
{
    [System.Serializable]
    public class FittingBaseType
    {
        public string name;
        public GameObject prefab;
    }

    /// <summary>
    /// 配件类型
    /// </summary>
    public enum FittingType
    {
        Mag,
        Grip,
        Scope,
        Muzzle,
        GunStock,
        TarticalEquipment
    }
}