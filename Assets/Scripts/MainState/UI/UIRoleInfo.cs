using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using Data;
using System.Collections.Generic;
using DefaultNamespace;
using DG.Tweening;
using UI;

public class UIRoleInfo : UIBase
{
    public Text txtName;
    public Text txtJobName;
    public Image imgRole;
    public GameObject gridFeature;
    public Text txtProp;
    public GameObject gridSkills;
    public GameObject gridEnableSkill;
    public Button btnClose;
    public GameObject pfbSkill;


    List<UIItemSkill> lstUIItemSkill;
    List<UIItemSkill> lstUIItemSkillsEnable;

    CharacterForRaid mCurSelectedChara;
    UIItemSkill mCurSelectedUIItemSkill;


    public override void Init()
    {
        base.Init();
        lstUIItemSkill = new List<UIItemSkill>();
        lstUIItemSkillsEnable = new List<UIItemSkill>();
        btnClose.onClick.AddListener(OnBtnClose);
    }

    public void SetData(CharacterForRaid chara)
    {
        mCurSelectedChara = chara;
    }

    public void Refresh() 
    {
        if (mCurSelectedChara == null)
        {
            return;
        }
        txtName.text = mCurSelectedChara.roleData.name;
        txtJobName.text = mCurSelectedChara.roleData.JobData.name;
        //TODO 特性
        
        txtProp.text = mCurSelectedChara.roleData.GetPropDesc();
        imgRole.SetSprite(GameUtil.ToTitleCase(mCurSelectedChara.roleData.model) + "/idle");
        imgRole.SetSpriteNativeSize(-1, 192);
        imgRole.DOColor(new Color(1, 1, 1, 1), 0.2f).From(new Color(1, 1, 1, 0));
        imgRole.rectTransform.DOLocalMoveX(0f, 0.2f).From(218f);
        
        //技能
        ClearSKillItems();
        var lstSkillEquiped = mCurSelectedChara.GetSkillList();
        for (int i = 0; i < lstSkillEquiped.Count; i++)
        {
            SkillBaseData skillData  = lstSkillEquiped[i];
            var skillItem = UIItemBase.Create<UIItemSkill>(gridSkills.transform, pfbSkill);
            skillItem.Init(skillData, OnHoverEnterSkillItem, OnHoverExitSkillItem);
            skillItem.skillIndex = i;
            skillItem.SetCBClick(OnClickSkillItemEquiped);
            skillItem.Refresh();

            lstUIItemSkill.Add(skillItem);
        }
        //可用技能
        var lstSkillsGetted = WorldRaidData.Inst.GetSkillsGetted();
        for (int i = -1; i < lstSkillsGetted.Count; i++)
        {
            SkillBaseData skillData = null;
            if (i >= 0)
            {
                skillData = lstSkillsGetted[i];
            }
            var skillItem = UIItemBase.Create<UIItemSkill>(gridEnableSkill.transform, pfbSkill);
            skillItem.Init(skillData, OnHoverEnterSkillItem, OnHoverExitSkillItem);
            skillItem.showSkillName = false;
            skillItem.SetCBClick(OnClickSkillItemNotEquiped);
            if (mCurSelectedChara.IsSkillEquiped(skillData))
            {
                skillItem.arrivable = false;
            }
            skillItem.Refresh();
            lstUIItemSkillsEnable.Add(skillItem);
        }
    }

    /// <summary>
    /// 点击未装备的技能
    /// </summary>
    /// <param name="obj"></param>
    private void OnClickSkillItemNotEquiped(UIItemSkill uiitem)
    {
        if (mCurSelectedUIItemSkill == null)
        {
            return;
        }
        var skillData = uiitem.Data;
        if (mCurSelectedChara.IsSkillEquiped(skillData))
        {
            return;
        }
        //替换为指定技能
        mCurSelectedChara.SetSkill(mCurSelectedUIItemSkill.skillIndex, skillData);

        mCurSelectedUIItemSkill.Data = skillData;
        mCurSelectedUIItemSkill.Refresh();
        if (skillData != null)
        {
            uiitem.arrivable = false;
            uiitem.RefreshArrivable();
        }
    }

    /// <summary>
    /// 点击已装备的技能
    /// </summary>
    /// <param name="obj"></param>
    private void OnClickSkillItemEquiped(UIItemSkill uiitem)
    {
        foreach (var item in lstUIItemSkill)
        {
            item.selected = false;
            item.RefreshSelected();
        }
        uiitem.selected = true;
        uiitem.RefreshSelected();
        mCurSelectedUIItemSkill = uiitem;
    }

    private void OnHoverExitSkillItem(UIItemSkill obj)
    {
        UIMgr.Inst.HideUI(UITable.EUITable.UITip);
    }

    private void OnHoverEnterSkillItem(UIItemSkill obj)
    {
        if (obj.Data != null)
        {
            var uiTip = UIMgr.Inst.ShowUI(UITable.EUITable.UITip) as UITip;
            uiTip.Refresh(obj.Data.tip);
        }
    }

    public override void OnShow()
    {
        base.OnShow();
    }

    public override void OnHide()
    {
        ClearSKillItems();
        base.OnHide();
    }

    void ClearSKillItems()
    {
        foreach (var item in lstUIItemSkill)
        {
            item.Cache();
        }
        lstUIItemSkill.Clear();
        foreach (var item in lstUIItemSkillsEnable)
        {
            item.Cache();
        }
        lstUIItemSkillsEnable.Clear();
    }

    private void OnBtnClose()
    {
        UIMgr.Inst.HideUI(UITable.EUITable.UIRoleInfo);
    }
}
