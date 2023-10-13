using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NativeSerializableDictionary;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace MVVM.BaseSO
{
    /// <summary>
    /// 主要用来标识某把武器、装备等由谁持有，主要用于Model与模块之间的通信
    /// </summary>
    [CreateAssetMenu(fileName = "CharactotSO_IDCard", menuName = "SO/Charactor/Character IDCard")]
    public class Charactor_IDCard_SO : ScriptableObject
    {
        public CharactorIDCard idCard;
        public PlayerSO charactorConfige;
    }

    
    [System.Serializable]
    public struct CharactorIDCard
    {
        public CharactorType nameType;
        public Sprite head_Photo;
        public GameObject prefab;
    }
}