namespace Equipments.BaseSO
{
    [System.Serializable]
    public class Muzzle : FittingBaseType
    {
        public FittingType fittingType => FittingType.Muzzle;
        public float RecoilReduce; //后坐力减免
        //...
    }
}