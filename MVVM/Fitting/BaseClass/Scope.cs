namespace Equipments.BaseSO
{
    [System.Serializable]
    public class Scope : FittingBaseType
    {
        public FittingType fittingType => FittingType.Scope;
        public float Magnification;//放大倍率
                                   //....
    }
}