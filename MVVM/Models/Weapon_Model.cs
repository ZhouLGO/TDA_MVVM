using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MVVM;
using MVVM.WeaponModel;
using UI.MVVM.Json_Asset;
using UnityEngine.Assertions;


namespace UI.MVVM.Models
{
    public class Weapon_Model : IModel,IWeapon_Model
    {
        private bool Debug = true;

        public void SelfSave()
        {
            Asset_Saver_Loader.Save_JsonToPrsistentPath(weaponAsset, weaponJsonPath);
        }
        public Weapon_Model()
        {
            if (Debug || !Asset_Saver_Loader.Load_FromPersistentPath(ref weaponAsset, weaponJsonPath))
            {
                //从本地加载存档失败
                weaponAsset = new PlayerWeaponAssets(); //则直接在内存里初始化一个存档
                //保存文件
                if(!Debug) //Debug模式下不保存本地文件
                Asset_Saver_Loader.Save_JsonToPrsistentPath(weaponAsset, weaponJsonPath);
            }
            else UnityEngine.Debug.Log("weaponAsset_Json读取成功");
            
            weaponAsset.InitWeaponEquipDictionary();
                
            //初始化当前激活角色的武器库
            CurrentActiveCharactrorWeaponLib = GetCharactorEquipWeapons(CurrentActiveCharactor);
            
            //注册角色Model当前激活角色属性的事件
            GameManager.Ins.SourcesManager.CharactorModel.M_PropertyChanged += (sender, e) =>
            {
                CurrentActiveCharactrorWeaponLib = GetCharactorEquipWeapons(CurrentActiveCharactor);
            };
        }

                
        #region Model层的核心

        public event PropertyChangedEventHandler M_PropertyChanged;

        public void NotifyModelPropertyChanged(string propertyName)
        {
            M_PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        #endregion
        
        #region 武器存档相关，数据同步事件等杂七杂八的

        private static readonly string weaponJsonPath = "/PlayerAsset/PlayerWeaponLibAsset.json";

        private PlayerWeaponAssets weaponAsset = null;

        public Dictionary<string, A_Weapon>.Enumerator WeaponAssetsEnumerator =>
            weaponAsset.iD_Dictionary.GetEnumerator();
        public CharactorType CurrentActiveCharactor => GameManager.Ins.SourcesManager.CharactorModel.CurrentCharactor;

        private ObservableCollection_AWeapon currentActiveCharactrorWeaponLib;
        public ObservableCollection_AWeapon CurrentActiveCharactrorWeaponLib
        {
            get => currentActiveCharactrorWeaponLib;
            set
            {
                if (currentActiveCharactrorWeaponLib != value)
                {
                    //TODO:为新传入的角色当前装备武器库注册事件，为旧的角色当前装备武器库注销事件
                    value.CollectionChanged += ListenCurrentWeaponLibChanged;
                    if (currentActiveCharactrorWeaponLib != null)
                    {
                        currentActiveCharactrorWeaponLib.CollectionChanged -= ListenCurrentWeaponLibChanged;
                    }
                    currentActiveCharactrorWeaponLib = value;
                    NotifyModelPropertyChanged(nameof(CurrentActiveCharactrorWeaponLib));
                }
            }
        }

        public ObservableCollection_AWeapon GetCharactorEquipWeapons(CharactorType who)
        {
            Assert.IsTrue(weaponAsset.allCharactorEquipInfo.ContainsKey(who), $"角色武器配置列表不存在角色{who}");
            return weaponAsset.allCharactorEquipInfo[who];
        }
        private void ListenCurrentWeaponLibChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            //注册当前角色装备武器库变化事件
            NotifyModelPropertyChanged(nameof(CurrentActiveCharactrorWeaponLib));
            WeaponHUD_View.DebugLog("当前操作角色武器库里装备的枪发生改变");
        }
        #endregion
        
        #region Model层属性
        
        public A_Weapon MainWeapon
        {
            get=> CurrentActiveCharactrorWeaponLib.Count>0 ? CurrentActiveCharactrorWeaponLib[0] : null;
            set
            {
                CurrentActiveCharactrorWeaponLib[0] = value;
                NotifyModelPropertyChanged(nameof(MainWeapon));
            }
        }

        public A_Weapon DeputyWeapon
        {
            get => CurrentActiveCharactrorWeaponLib.Count>1 ? CurrentActiveCharactrorWeaponLib[1] : null;
            set
            {
                CurrentActiveCharactrorWeaponLib[1] = value;
                NotifyModelPropertyChanged(nameof(DeputyWeapon));
            }
        }

        private A_Weapon activeWeapon;

        public A_Weapon ActiveWeapon
        {
            get => activeWeapon;
            set
            {
                activeWeapon = value;
                NotifyModelPropertyChanged(nameof(ActiveWeapon));
            }
        }

        public int CurrentClipAmmo
        {
            get =>ActiveWeapon?.currentAmmoCount ?? 0;
        
            set
            {
                ActiveWeapon.currentAmmoCount = value;
                NotifyModelPropertyChanged(nameof(CurrentClipAmmo));
            }
        }
        
        public int ReserveAmmo
        {
            get => ActiveWeapon == null ? 0 : GetCurrentTypeDeputyAmmo(ActiveWeapon.BaseSO.GetWeaponType());
            set
            {
                SetCurrentTypeDeputyAmmo(ActiveWeapon.BaseSO.GetWeaponType(),value);
                NotifyModelPropertyChanged(nameof(ReserveAmmo));
            }
        }

        #endregion

        #region 工具方法

        private int GetCurrentTypeDeputyAmmo(WeaponType type)
        {
            Assert.IsTrue(weaponAsset.ammoLibrary.ContainsKey(type),$"弹药字典中不存在类型为{type}的弹药");
            return weaponAsset.ammoLibrary[type];
        }

        private void SetCurrentTypeDeputyAmmo(WeaponType type,int value)
        {
            Assert.IsTrue(weaponAsset.ammoLibrary.ContainsKey(type),$"弹药字典中不存在类型为{type}的弹药"); 
            weaponAsset.ammoLibrary[type] = value;
        }
        #endregion

        #region 武器Model事件及操作接口
        
        public event Action<A_Weapon,WeaponModelOperationType> OnWeaponAssetsChanged;
        
        public void AddWeapon(A_Weapon newWeapon)
        {
            newWeapon.BeUnload();
            weaponAsset.iD_Dictionary.TryAdd(newWeapon.hashID,newWeapon);
            NotifyModelPropertyChanged(nameof(AddWeapon));
            OnWeaponAssetsChanged?.Invoke(newWeapon,WeaponModelOperationType.Add);
        }

        public void ThrowWeapon(A_Weapon whichOne)
        {
            if (weaponAsset.iD_Dictionary.ContainsKey(whichOne.hashID)) ;
            {
                //丢弃武器这里先通知，先让外部解除对这把武器的引用，防止空引用报错
                OnWeaponAssetsChanged?.Invoke(whichOne,WeaponModelOperationType.Throw);

                //Remove内存中的装备
                if (weaponAsset.allCharactorEquipInfo.Any(key => key.Key == whichOne.holder.Value))
                {
                    weaponAsset.allCharactorEquipInfo[whichOne.holder.Value].Remove(whichOne);
                    if (whichOne.holder.Value == CurrentActiveCharactor)
                    {
                        //通知HUD刷新
                        NotifyModelPropertyChanged(nameof(CurrentActiveCharactrorWeaponLib));
                    }
                }
                //从武器库中Remove这个Weapon对象
                weaponAsset.iD_Dictionary.Remove(whichOne.hashID);
                
                NotifyModelPropertyChanged(nameof(ThrowWeapon));
            }
        }

        public void TakeWeapon(A_Weapon whichOne)
        {
            if (whichOne.holder.Value != CharactorType.None)
                throw new Exception("这把武器正在被装备，无法被临时装备");
            whichOne.BeEuiped(CurrentActiveCharactor);
            NotifyModelPropertyChanged(nameof(TakeWeapon));
            OnWeaponAssetsChanged?.Invoke(whichOne,WeaponModelOperationType.Taking);
        }

        public void DestroyWeapon(A_Weapon whichOne)
        {
            if (weaponAsset.iD_Dictionary.ContainsKey(whichOne.hashID))
            {
                if (whichOne.holder.Value != CharactorType.None)
                    throw new Exception("这把武器正在被装备，无法被销毁");
                weaponAsset.iD_Dictionary.Remove(whichOne.hashID);
                NotifyModelPropertyChanged(nameof(DestroyWeapon));
                OnWeaponAssetsChanged?.Invoke(whichOne,WeaponModelOperationType.Destroy);
            }
        }
        
        public enum WeaponModelOperationType
        {
            Add,
            Throw,
            Taking,
            Destroy
        }
        #endregion
    }
}
