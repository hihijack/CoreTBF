using UnityEngine;
using System.Collections;
using Data;
using System;
using UnityEngine.UI;

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

    public void Init(SkillBaseData data, Action<UIItemSkill> onHoverEnter, Action<UIItemSkill> onHoverExit)
    {
        Data = data;
        this.onHoverEnter = onHoverEnter;
        this.onHoverExit = onHoverExit;
    }

    public void SetCBClick(Action<UIItemSkill> onClick)
    {
        this.onClick = onClick;
    }

    public override void Refresh()
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
        onHoverEnter(this);
    }

    public void OnHoverExit()
    {
        onHoverExit(this);
    }

    public void OnBtnClick()
    {
        onClick?.Invoke(this);
    }
}
