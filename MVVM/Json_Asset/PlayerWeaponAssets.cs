using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using DACore.DAWeaponData;
using NativeSerializableDictionary;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace UI.MVVM.Json_Asset
{
    /// <summary>
    /// 玩家的武器资产类，会被Json化存储在玩家电脑本地磁盘，在游戏启动时加载到内存里
    /// </summary>
    [System.Serializable]
    public class PlayerWeaponAssets
    {
        //游戏运行时在程序内存里的数据对象
        //key:玩家仓库中每把武器的唯一ID编号，在A_Weapon的构造方法中生成
        /// <summary>
        /// 存档的Core，玩家的所有固有资产都由此来
        /// </summary>
        public SerializableDictionary<string, A_Weapon> iD_Dictionary;
        
        /// <summary>
        /// 玩家存档中所有角色配置的武器列表，在加载存档后初始化
        /// 可监视的，当你向该容器Add或Remove时，会同步武器上的持有信息
        /// </summary>
        [NonSerialized] public Dictionary<CharactorType, ObservableCollection_AWeapon> allCharactorEquipInfo;
        
        /// <summary>
        /// 玩家的弹药库，可Json本地存储
        /// </summary>
        public SerializableDictionary<WeaponType, int> ammoLibrary;
        
        /// <summary>
        /// 当本地Json文件加载失败时调用New初始化一个新武器库存档，或开发阶段时
        /// </summary>
        public PlayerWeaponAssets()
        {
            var SOlibrary = GameManager.Ins.SourcesManager.Weapon_SO.Library;
            var testWeapon1 = CreatWeapon(SOlibrary, name: "HK416",
                whoTaking: new SerializableKVP<bool, CharactorType>(true, CharactorType.HK416));
            var testWeapon2 = CreatWeapon(SOlibrary, name: "M1911",
                whoTaking: new SerializableKVP<bool, CharactorType>(false, CharactorType.None));
            var testWeapon3 = CreatWeapon(SOlibrary, name: "AK47",
                whoTaking: new SerializableKVP<bool, CharactorType>(false, CharactorType.None));
            var testWeapon4 = CreatWeapon(SOlibrary, name: "Vector",
                whoTaking: new SerializableKVP<bool, CharactorType>(true, CharactorType.HK416));
            var testWeapon5 = CreatWeapon(SOlibrary, name: "M1911",
                whoTaking: new SerializableKVP<bool, CharactorType>(false, CharactorType.None));
            var testWeapon6 = CreatWeapon(SOlibrary, name: "M1911",
                whoTaking: new SerializableKVP<bool, CharactorType>(false, CharactorType.None));
            var testWeapon7 = CreatWeapon(SOlibrary, name: "M1911",
                whoTaking: new SerializableKVP<bool, CharactorType>(false, CharactorType.None));

            
            iD_Dictionary = new SerializableDictionary<string, A_Weapon>()
            {
                { testWeapon1.hashID,testWeapon1},
                { testWeapon2.hashID,testWeapon2},
                { testWeapon3.hashID,testWeapon3},
                { testWeapon4.hashID,testWeapon4},
                { testWeapon5.hashID,testWeapon5},
                { testWeapon6.hashID,testWeapon6},
                { testWeapon7.hashID,testWeapon7}
                //....
            };
            
            ammoLibrary = new SerializableDictionary<WeaponType, int>()
            {
                { WeaponType.Pistol,120}, //默认120手枪弹药
                { WeaponType.LongGun,300} //默认300发的突击步枪弹药
            };
        }

        private A_Weapon CreatWeapon(SerializedDictionary<string, DAWeaponSO> SO_library,string name,SerializableKVP<bool, CharactorType> whoTaking)
        {
            if (SO_library.TryGetValue(name, out DAWeaponSO so))
            {
                var testWeapon1 = new A_Weapon(so,holder_:whoTaking);
                return testWeapon1;
            }
            else
            {
                Debug.LogError($"没有找到名为{name}的武器,去检查SO_Entity/WeaponSO_Library里是否有这个名字的武器");
            }
            return null;
        }
        
        /// <summary>
        /// 初始化所有角色的装备字典（分配内存，然后遍历武器库）
        /// </summary>
        public void InitWeaponEquipDictionary()
        {
            allCharactorEquipInfo = new Dictionary<CharactorType, ObservableCollection_AWeapon>();
           
            var charactorSO = GameManager.Ins.SourcesManager.Character_Library;
            foreach (var charactor in charactorSO.id_Library.Keys)
            {
                allCharactorEquipInfo.TryAdd(charactor,new ObservableCollection_AWeapon(charactor));
            }

            InitWeaponEquipInfo();
        }

        /// <summary>
        /// Linq遍历每一把武器的Equip的bool值，然后初始化字典内的可监视列表
        /// </summary>
        private void InitWeaponEquipInfo()
        {
            var charactorSO = GameManager.Ins.SourcesManager.Character_Library;
            //遍历每一把KVP的bool key值为true（被装备）的武器
            foreach (A_Weapon e_weapon in iD_Dictionary.Values.Where(equiped => equiped.holder.Key == true).ToList())
            {
                Assert.IsTrue(charactorSO.id_Library.ContainsKey(e_weapon.holder.Value),$"这把武器的装备boolen值为true，但是它的holder属性值为{e_weapon.holder.Value}，这个值是不存在的角色类型");

                if (allCharactorEquipInfo[e_weapon.holder.Value].Count >= 2)
                {
                    e_weapon.BeUnload();
                    continue;
                }
                
                //根据KVP的enum Value值将这把枪初始化到武器配置字典中
                switch (e_weapon.holder.Value)
                {
                    case CharactorType.HK416:
                        allCharactorEquipInfo[CharactorType.HK416].Add(e_weapon);
                        break;
                    case CharactorType.G36:
                        allCharactorEquipInfo[CharactorType.G36].Add(e_weapon);
                        break;
                    case CharactorType.Vector:
                        allCharactorEquipInfo[CharactorType.Vector].Add(e_weapon);
                        break;
                    //TODO：其他角色后续再加上
                }
            }
        }


        private void CheckCharactorKeyExist(CharactorType who)
        {
            Assert.IsTrue(allCharactorEquipInfo.ContainsKey(who), $"角色武器配置列表不存在角色{who}");
        }
    }

    public class ObservableCollection_AWeapon : ObservableCollection<A_Weapon>
    {
        public ObservableCollection_AWeapon(CharactorType who) : base()
        {
            Who = who;
        }
        public ObservableCollection_AWeapon(CharactorType who,List<A_Weapon> list) : base(list)
        {
            Who = who;
        }

        public CharactorType Who { get;private set; }


        /// <summary>
        /// 角色武器槽Add时自动把Weapon对象上的holder属性修改了
        /// </summary>
        /// <param name="index"></param>
        /// <param name="weapon"></param>
        protected override void InsertItem(int index, A_Weapon weapon)
        {
            base.InsertItem(index, weapon);
            weapon?.BeEuiped(Who);
            CharactorEWeaponLibChanged?.Invoke(weapon, WeaponOperationType.Install);
        }

        /// <summary>
        /// 角色卸下武器时自动把Weapon对象上的holder属性修改了
        /// </summary>
        /// <param name="index"></param>
        protected override void RemoveItem(int index)
        {
            this[index].BeUnload();
            var targetWeapon = this[index];//留一下引用，最后才通知外界变化来访问数组
            base.RemoveItem(index);
            CharactorEWeaponLibChanged?.Invoke(targetWeapon, WeaponOperationType.Uninstall);
        }

        protected override void ClearItems()
        {
            foreach (var weapon in Items)
            {
                weapon.BeUnload();
                CharactorEWeaponLibChanged?.Invoke(weapon, WeaponOperationType.Uninstall);
            }
            base.ClearItems();
        }

        public event Action<A_Weapon, WeaponOperationType> CharactorEWeaponLibChanged;
        public enum WeaponOperationType
        {
            Install,
            Uninstall
        }
    }
}
