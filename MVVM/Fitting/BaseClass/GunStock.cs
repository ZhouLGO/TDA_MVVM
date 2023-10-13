namespace Equipments.BaseSO
{
    [System.Serializable]
    public class GunStock : FittingBaseType
    {
        public FittingType fittingType => FittingType.GunStock;
        public float RecoilReduce; //
        public float Ergonomics; //˻Ч
                                    //...
    }
}