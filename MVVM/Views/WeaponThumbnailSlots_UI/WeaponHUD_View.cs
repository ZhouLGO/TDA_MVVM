using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVVM;
using UnityEngine.UI;
using System.ComponentModel;
using TMPro;

public class WeaponHUD_View : View<WeaponHUD_ViewModel>
{
    public override View<WeaponHUD_ViewModel> ParentView => null;
    public override IViewModel ViewModel { get => _viewModel; }
    private WeaponHUD_ViewModel _viewModel;
    public Image[] WeaponIcons = new Image[2];
    public TextMeshProUGUI[] E_WeaponsTypeTMP = new TextMeshProUGUI[2];
    public TextMeshProUGUI[] E_WeaponsName = new TextMeshProUGUI[2];
    public Image[] WeaponBG = new Image[2]; 
    readonly Color defaultBGColor = new Color(0x37 / 255f, 0x37 / 255f, 0x37 / 255f, 200 / 255f);
    public TextMeshProUGUI CurrentAmmoCountTMP;
    public TextMeshProUGUI TypeAmmoCountTMP;

    private void Start()
    {
        _viewModel = new WeaponHUD_ViewModel();
        //绑定View
        ViewModel.VM_PropertyChanged += Update_View_Info;

        //TODO:获知当前手上拿的哪把装备的武器，并刷新UI
        RefreshView();

        //TODO:初始化单例类UIManager的HUD属性
        UIManager.Instance.WeaponHUDView = this;
    }

    /// <summary>
    /// 当外部的IsSwapWeapon方法执行时，从OnSwitchWeapon委托那里把信息传过来，让该方法知道切换到哪哪枪，再回调OnSwitchWeapon委托
    /// </summary>
    /// <param name="index">/默认这里玩家按数字键1、2对应的编号是0，1，当外部方法改的话记得回来改逻辑</param>
    /// <param name="targetWeapon"></param>
    public void Set_BeTakenWeapon_BGcolor()
    {
        ///Error Cause：对外依赖的PlayerWeaponManager组件修改了
        //背景板颜色全部重置
        WeaponBG[0].color = defaultBGColor;
        WeaponBG[1].color = defaultBGColor;

        //目标武器HUD的槽背景色设为橙色(如果没有与之对应的武器，就不改变背景色(预想卸下武器/使用场景武器))
        int? index = null;
        if (_viewModel.MainWeapon != null && _viewModel.ActiveWeapon == _viewModel.MainWeapon) index = 0;
        else if (_viewModel.DeputyWeapon != null && _viewModel.ActiveWeapon == _viewModel.DeputyWeapon) index = 1;
        if (index != null)
            WeaponBG[(int)index].color = new Color(0xFF / 255f, 0x66 / 255f, 0x00 / 255f, 1);
    }

    public override void RefreshView()
    {
        InitWeaponSlot_1();
        InitWeaponSlot_2();
        Set_BeTakenWeapon_BGcolor();
        RefreshAmmoCountTMP();
    }

    public override void Update_View_Info(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_viewModel.MainWeaponSprite))
        {
            WeaponIcons[0].sprite = _viewModel.MainWeaponSprite;
            if (_viewModel.MainWeaponSprite != null) WeaponIcons[0].color = new Color(1, 1, 1, 1);
            else//若该槽位卸下装备
            {
                WeaponIcons[0].color = new Color(1, 1, 1, 0);
                WeaponBG[0].color = defaultBGColor;
            }
        }
        else if (e.PropertyName == nameof(_viewModel.MainWeaponType))
        {
            E_WeaponsTypeTMP[0].text = _viewModel.MainWeaponType;
        }
        else if (e.PropertyName == nameof(_viewModel.MainWeaponName))
        {
            E_WeaponsName[0].text = _viewModel.MainWeaponName;
        }
        else if (e.PropertyName == nameof(_viewModel.DeputyWeaponSprite))
        {
            WeaponIcons[1].sprite = _viewModel.DeputyWeaponSprite;
            if (_viewModel.DeputyWeaponSprite != null) WeaponIcons[1].color = new Color(1, 1, 1, 1);
            else//若该槽位卸下装备
            {
                WeaponIcons[1].color = new Color(1, 1, 1, 0);
                WeaponBG[1].color = defaultBGColor;
            }
        }
        else if (e.PropertyName == nameof(_viewModel.DeputyWeaponType))
        {
            E_WeaponsTypeTMP[1].text = _viewModel.DeputyWeaponType;
        }
        else if (e.PropertyName == nameof(_viewModel.DeputyWeaponName))
        {
            E_WeaponsName[1].text = _viewModel.DeputyWeaponName;
        }
        else if (e.PropertyName == nameof(_viewModel.ActiveWeapon))
        {
            RefreshAmmoCountTMP();
            Set_BeTakenWeapon_BGcolor();
        }
        else if (e.PropertyName == nameof(_viewModel.CurrentWeaponAmmo)
                 || e.PropertyName == nameof(_viewModel.CurrentReserveTypeAmmo))
        {
            RefreshAmmoCountTMP();
        }
    }

    private void InitWeaponSlot_1()
    {
        //武器剪影图标
        if (_viewModel.MainWeapon != null)
        {
            WeaponIcons[0].sprite = _viewModel.MainWeaponSprite;
            WeaponIcons[0].color = new Color(1, 1, 1, 1);
        }
        else { WeaponIcons[0].sprite = null; WeaponIcons[0].color = new Color(1, 1, 1, 0); }

        WeaponBG[0].color = defaultBGColor;
        WeaponIcons[0].preserveAspect = true;

        E_WeaponsTypeTMP[0].text = _viewModel.MainWeaponType;//可接受空

        E_WeaponsName[0].text = _viewModel.MainWeaponName;//可接受空
    }
    private void InitWeaponSlot_2()
    {
        if (_viewModel.DeputyWeapon != null)
        { 
            WeaponIcons[1].sprite = _viewModel.DeputyWeaponSprite;
            WeaponIcons[1].color = new Color(1, 1, 1, 1);
        }
        else { WeaponIcons[1].sprite = null; WeaponIcons[1].color = new Color(1, 1, 1, 0); }

        WeaponBG[1].color = defaultBGColor;
        WeaponIcons[1].preserveAspect = true;

        E_WeaponsTypeTMP[1].text = _viewModel.DeputyWeaponType;

        E_WeaponsName[1].text = _viewModel.DeputyWeaponName;
    }

    public void RefreshAmmoCountTMP()
    {
        //Error Cause：对外依赖的PlayerWeaponManager组件删除了
        if (_viewModel.ActiveWeapon != null)
        {
            CurrentAmmoCountTMP.text = _viewModel.CurrentWeaponAmmo.ToString();
            TypeAmmoCountTMP.text = _viewModel.CurrentReserveTypeAmmo.ToString();
        }
        else
        {
            CurrentAmmoCountTMP.text = "N/A";
            TypeAmmoCountTMP.text = "N/A";
        }
    }
    
    public static void DebugLog(string log)
    {
        Debug.Log(log);
    }
    public void OnGUI()
    {
        if(GUILayout.Button("点击手动保存"))
        {
            _viewModel.SelfSave();
        }
    }
}
