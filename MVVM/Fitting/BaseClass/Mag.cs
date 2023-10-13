
namespace Equipments.BaseSO
{
    [System.Serializable]
    public class Mag : FittingBaseType
    {
        public FittingType fittingType => FittingType.Mag;
        public int maxAmmoCount;
        public int currentAmmoCount;
        public float reloadTime;
    }
}


