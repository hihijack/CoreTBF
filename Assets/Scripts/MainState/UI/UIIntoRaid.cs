using Boo.Lang;
using Data;
using DG.Tweening;
using System;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class UIIntoRaid : UIBase
{
    public Button btnClose;
    public Button btnComfirm;
    public GameObject gridCharacters;
    public GameObject gridSkill;
    public GameObject pfbCharacterItem;
    public GameObject pfbSkillItem;
    public Image imgCharacter;
    public Text txtName;
    public Text txtProp;

    List<UIItemCharacterSimple> lstUIItemCharacter;
    List<UIItemSkill> lstUIItemSkill;
    RoleBaseData mCurSelectedRoledata;

    public override void Init()
    {
        base.Init();
        lstUIItemCharacter = new List<UIItemCharacterSimple>();
        lstUIItemSkill = new List<UIItemSkill>();
        btnClose.onClick.AddListener(OnBtnClose);
        btnComfirm.onClick.AddListener(OnBtnComfirm);
    }

    public override void OnShow()
    {
        base.OnShow();
        //角色列表
        var lst = PlayerDataMgr.Inst.PlayerData.LstCharacterIDUnlocked;
        for (int i = 0; i < lst.Count; i++)
        {
            var roleData = RoleDataer.Inst.Get(lst[i]);
            var uiItem = UIItemBase.Create<UIItemCharacterSimple>(gridCharacters.transform, pfbCharacterItem);
            uiItem.Init(roleData, OnClickCharacterItem);
            uiItem.Refresh();
            uiItem.SetSelected(false);
            lstUIItemCharacter.Add(uiItem);
        }
        OnClickCharacterItem(lstUIItemCharacter[0]);
    }

    public override void OnHide()
    {
        base.OnHide();
        foreach (var item in lstUIItemCharacter)
        {
            item.Cache();
        }
        foreach (var item in lstUIItemSkill)
        {
            item.Cache();
        }
    }

    private void OnClickCharacterItem(UIItemCharacterSimple target)
    {
        foreach (var item in lstUIItemCharacter)
        {
            item.SetSelected(false);
        }
        target.SetSelected(true);
        mCurSelectedRoledata = target.Data;
        Refresh();
    }

    private void Refresh()
    {
        imgCharacter.SetSprite(GameUtil.ToTitleCase(mCurSelectedRoledata.model) + "/idle");
        imgCharacter.SetSpriteNativeSize(370, -1);
        imgCharacter.DOColor(new Color(1, 1, 1, 1), 0.2f).From(new Color(1,1,1,0));
        imgCharacter.rectTransform.DOLocalMoveX(-25f, 0.2f).From(0f);
        txtName.text = mCurSelectedRoledata.name;
        txtProp.text = mCurSelectedRoledata.GetPropDesc();
        //刷新技能
        foreach (var item in lstUIItemSkill)
        {
            item.Cache();
        }
        lstUIItemSkill.Clear();
        for (int i = 0; i < mCurSelectedRoledata.skills.Length; i++)
        {
            var skillID = mCurSelectedRoledata.skills[i];
            if (skillID > 0)
            {
                var skillData = SkillDataer.Inst.Get(skillID);
                var skillItem = UIItemBase.Create<UIItemSkill>(gridSkill.transform, pfbSkillItem);
                skillItem.Init(skillData, OnHoverEnterSkillItem, OnHoverExitSkillItem);
                skillItem.Refresh();
                lstUIItemSkill.Add(skillItem);
            }
        }
    }

    private void OnHoverExitSkillItem(UIItemSkill obj)
    {
        UIMgr.Inst.HideUI(UITable.EUITable.UITip);    
    }

    private void OnHoverEnterSkillItem(UIItemSkill obj)
    {
        var uiTip =  UIMgr.Inst.ShowUI(UITable.EUITable.UITip) as UITip;
        uiTip.Refresh(obj.Data.tip);
    }

    private void OnBtnComfirm()
    {
        int[] roles = new int[]{mCurSelectedRoledata.ID};
        int numOfFood = 30;
         GameMgr.Inst.MainState.curInWorld = EWorld.OtherWorld;
        WorldRaidData.Inst.Init(roles, numOfFood);
        UIMgr.Inst.ShowUI(UITable.EUITable.UIWorldTree);
        UIMgr.Inst.ShowUI(UITable.EUITable.UIWorldInfo);
        UIMgr.Inst.HideUI(UITable.EUITable.UIIntoRaid);
    }

    private void OnBtnClose()
    {
        UIMgr.Inst.ShowUI(UITable.EUITable.UIMainStage);
        UIMgr.Inst.HideUI(UITable.EUITable.UIIntoRaid);
    }

}
