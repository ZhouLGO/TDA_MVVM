using NativeSerializableDictionary;
using System.Collections.Generic;
using UnityEngine;

namespace Equipments.BaseSO
{
    [System.Serializable]
    public class TarticalEquipment : FittingBaseType
    {
        public FittingType fittingType => FittingType.TarticalEquipment;

        //...
    }

    /// <summary>
    /// 镭射
    /// </summary>
    [System.Serializable]
    public class LaserEquipment : TarticalEquipment
    {
        //镭射颜色
        public Color color;
        //是否红外
        //电量
    }

    /// <summary>
    /// 战术灯
    /// </summary>
    [System.Serializable]
    public class LightEquipment : TarticalEquipment
    {
        //范围
        //光色
        public Color color;
        //电量
    }

    /// <summary>
    /// 子弹带
    /// </summary>
    [System.Serializable]
    public class BulletBeltEquipment : TarticalEquipment
    {
        //载弹数
        public int ammoCount;
    }
}