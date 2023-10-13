using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVVM.WeaponModel
{
    public interface IWeapon_Model
    {
        /// <summary>
        /// 向玩家资产库中永久添加某个新武器
        /// </summary>
        /// <param name="newWeapon">必须是一把武器库中ID不同的一件武器</param>
        void AddWeapon(A_Weapon newWeapon);
        /// <summary>
        /// 抛弃玩家武器资产库中的某个武器
        /// </summary>
        /// <param name="whichOne">这把武器可以是被装备的</param>
        void ThrowWeapon(A_Weapon whichOne);
        /// <summary>
        /// 临时装备某件武器（场景武器/关卡内敌人掉落的武器）
        /// </summary>
        /// <param name="whichOne">这件武器目前没人用</param>
        void TakeWeapon(A_Weapon whichOne);
        /// <summary>
        /// 销毁玩家武器库中的某件武器
        /// </summary>
        /// <param name="whichOne">这把武器不能被任何角色装备，否则会抛出一个Exception</param>
        void DestroyWeapon(A_Weapon whichOne);
    }
}

