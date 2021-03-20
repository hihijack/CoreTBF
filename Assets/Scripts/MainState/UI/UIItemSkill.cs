using UnityEngine;
using System.Collections;
using Data;
using System;
using UnityEngine.UI;
using UI;

public class UIItemSkill : UIItemBase
{
    public Button btn;
    public Image icon;
    public Text txtName;
    public Image imgSelect;

    public SkillBaseData Data { get; set; }

    public int skillIndex;

    Action<UIItemSkill> onHoverEnter;
    Action<UIItemSkill> onHoverExit;

    Action<UIItemSkill> onClick;

    /// <summary>
    /// 是否显示技能名。默认为是
    /// </summary>
    [HideInInspector]
    public bool showSkillName = true;

    [HideInInspector]
    public bool selected = false;

    /// <summary>
    /// 是否可用
    /// </summary>
    [HideInInspector]
    public bool arrivable = true;

    bool isUnKnowSkill = false;//未知的技能

    public override void Cache()
    {
        Data = null;
        this.onHoverEnter = null;
        this.onHoverExit = null;
        arrivable = true;
        selected = false;
        showSkillName = true;
        onClick = null;
        skillIndex = 0;
        isUnKnowSkill = false;
        base.Cache();
    }

    public void Init(SkillBaseData data, Action<UIItemSkill> onHoverEnter, Action<UIItemSkill> onHoverExit)
    {
        Data = data;
        this.onHoverEnter = onHoverEnter;
        this.onHoverExit = onHoverExit;
        isUnKnowSkill = false;
    }

    public void InitAsUnknowSkill()
    {
        isUnKnowSkill = true;
    }

    public void SetCBClick(Action<UIItemSkill> onClick)
    {
        this.onClick = onClick;
    }

    public void RemoveCBClick()
    {
        this.onClick = null;
    }

    public override void Refresh()
    {
        if (!isUnKnowSkill)
        {
            if (Data != null)
            {
                icon.SetSprite(Data.icon);
                if (showSkillName)
                {
                    txtName.text = Data.name;
                }
                else
                {
                    txtName.text = "";
                }
            }
            else
            {
                icon.sprite = null;
                if (showSkillName)
                {
                    txtName.text = "-";
                }
                else
                {
                    txtName.text = "";
                }
            }

            RefreshSelected();
            RefreshArrivable();
        }
        else
        {
            //未知技能
            icon.SetSprite("Buffs/Icon1_48");
            txtName.text = "";
        }
        
    }

    public void RefreshSelected()
    {
        if (selected)
        {
            imgSelect.SetImageAlpha(1);
        }
        else
        {
            imgSelect.SetImageAlpha(0);
        }
    }

    public void RefreshArrivable()
    {
        if (arrivable)
        {
            icon.color = Color.white;
        }
        else
        {
            icon.color = Color.gray;
        }
    }

    public void OnHoverEnter()
    {
        if (Data != null)
        {
            var uiTip = UIMgr.Inst.ShowUI(UITable.EUITable.UITip) as UITip;
            uiTip.Refresh(Data.tip);
        }

        onHoverEnter?.Invoke(this);
    }

    public void OnHoverExit()
    {
        UIMgr.Inst.HideUI(UITable.EUITable.UITip);
        onHoverExit?.Invoke(this);
    }

    public void OnBtnClick()
    {
        onClick?.Invoke(this);
    }
}
