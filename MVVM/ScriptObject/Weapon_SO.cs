using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DACore.DAWeaponData;

namespace MVVM.BaseSO
{
    [CreateAssetMenu(fileName = "WeaponSO_Library", menuName = "SO/Weapon/Weapon Library")]
    public class Weapon_SO : ScriptableObject
    {
        public SerializedDictionary<string, DAWeaponSO> Library;
    }
}