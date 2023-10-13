using System;
using System.Collections.Generic;
using Equipments;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class WeaponSlotBar_View : MonoBehaviour
{
    [SerializeField] private Image weaponIcon;
    [SerializeField] private SerializedDictionary<FittingType,Image> fittingIcons;
    [SerializeField] private TextMeshProUGUI weaponName;
    [SerializeField] private Image holder_HeadPhoto;
    [SerializeField] private TextMeshProUGUI holder_Name;
    private readonly Color defaultBGColor = new Color(0x37 / 255f, 0x37 / 255f, 0x37 / 255f, 200 / 255f);
    [SerializeField] private Button slotButton;
    
    //该UI槽所持有的武器
    private A_Weapon _weapon;
    public A_Weapon Weapon
    {
        get => _weapon;
        set
        {
            if (_weapon != value)
            {
                _weapon = value;
                
                RefreshSlotInfo();
            }
        }
    }

    private void Awake()
    {
        slotButton.onClick.AddListener(NotifySelectedSelf);
    }

    public event Action<WeaponSlotBar_View> OnBarClicked;
    private void NotifySelectedSelf()
    {
        OnBarClicked?.Invoke(this);
    }
    
    
    public void RefreshSlotInfo()
    {
        weaponIcon.sprite = Weapon.BaseSO.UI_Sources.vertical_Icon;
        weaponName.text = Weapon.BaseSO.weaponName;
        if (Weapon.holder.Key == true)
        {
            Assert.IsTrue(Weapon.holder.Value!=CharactorType.None,"该武器的装备bool值为true，但是装备者为None");
            Assert.IsTrue(GameManager.Ins.SourcesManager.Character_Library.id_Library.ContainsKey(Weapon.holder.Value),$"该武器的装备bool值为true，但是装备者为{Weapon.holder.Value.ToString()},她不在角色库中,去检查SOEntity文件夹下的CharactorLibrary");
            holder_Name.text = GameManager.Ins.SourcesManager.Character_Library.id_Library[Weapon.holder.Value].idCard.nameType.ToString();
            holder_HeadPhoto.color = Color.white;
            holder_HeadPhoto.sprite = GameManager.Ins.SourcesManager.Character_Library.id_Library[Weapon.holder.Value].idCard.head_Photo;
        }
        else
        {
            holder_Name.text = "Null";
            holder_HeadPhoto.color = defaultBGColor;
        }
        //TODO:刷新配件图标（预留位置）
        foreach (var fittingImage in fittingIcons.Values)
        {
            fittingImage.color = defaultBGColor;
        }
    }
}
