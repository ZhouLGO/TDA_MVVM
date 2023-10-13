using UnityEngine;
using System.Collections.Generic;

namespace MVVM.BaseSO
{
    [CreateAssetMenu(fileName = "CharactorSO_Library", menuName = "SO/Charactor/Character Library")]
    public class Charactor_Library_SO : ScriptableObject
    {
        public SerializedDictionary<CharactorType, Charactor_IDCard_SO> id_Library;
    }
}