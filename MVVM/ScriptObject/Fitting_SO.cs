using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Equipments.BaseSO;

namespace MVVM.BaseSO
{
    [CreateAssetMenu(fileName = "FittingSO_Library", menuName = "SO/Fitting/Fitting Library")]

    public class Fitting_SO : ScriptableObject
    {
        public List<Mag> mags;
        public List<Scope> scopes;
        public List<GunStock> gunStocks;
        public List<Muzzle> muzzles;
        public List<LaserEquipment> laserEquipments;
        public List<LightEquipment> lightEquipments;
        public List<BulletBeltEquipment> bulletBeltEquipments;
    }

}