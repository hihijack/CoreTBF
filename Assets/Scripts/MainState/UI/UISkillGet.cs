using Data;
using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public struct DataUISkillGet
{
    public int maxCount;
    public int getCount;
}

public class UISkillGet : UIBase
{
    public Button btnComfirm;
    public Text txtTip;
    public Transform tfGrid;
    public GameObject pfbSkillItem;

    DataUISkillGet data;

    int curCountGet;

    List<UIItemSkill> lstUIItemSkill;
    private bool enableOpenSkill;

    public void SetData(DataUISkillGet data)
    {
        this.data = data;
    }

    public override void Init()
    {
        base.Init();
        btnComfirm.onClick.AddListener(OnBtnComfirm);
        lstUIItemSkill = new List<UIItemSkill>();
    }

    public override void OnHide()
    {
        base.OnHide();
        foreach (var item in lstUIItemSkill)
        {
            item.Cache();
        }
        lstUIItemSkill.Clear();
    }

    private void OnBtnComfirm()
    {
        UIMgr.Inst.HideUI(UITable.EUITable.UISkillGet);
    }

    public void Refresh()
    {
        btnComfirm.interactable = false;
        curCountGet = 0;
        enableOpenSkill = true;
        txtTip.text = $"选择{data.getCount}个技能加入技能库";
        for (int i = 0; i < data.maxCount; i++)
        {
            var uiItemSkill = UIItemBase.Create<UIItemSkill>(tfGrid, pfbSkillItem);
            uiItemSkill.InitAsUnknowSkill();
            uiItemSkill.SetCBClick(OnClickSkill);
            uiItemSkill.Refresh();
            lstUIItemSkill.Add(uiItemSkill);
        }
    }

    private void OnClickSkill(UIItemSkill item)
    {
        if (!enableOpenSkill)
        {
            return;
        }
        //翻开，并获得技能
        SkillBaseData skillBaseData = WorldRaidData.Inst.GetARandomNewSkill();
        if (skillBaseData != null)
        {
            //获得技能
            WorldRaidData.Inst.AddSkillGetted(skillBaseData);
            item.Init(skillBaseData, null, null);
            item.RemoveCBClick();
            item.Refresh();
            AddCurGetCount();
        }
    }

    private void AddCurGetCount()
    {
        curCountGet++;
        if (curCountGet >= data.getCount)
        {
            btnComfirm.interactable = true;
            enableOpenSkill = false;
        }
    }
}
