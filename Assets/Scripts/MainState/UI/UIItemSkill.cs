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

    public SkillBaseData Data { get; set; }

    Action<UIItemSkill> onHoverEnter;
    Action<UIItemSkill> onHoverExit;

    public void Init(SkillBaseData data, Action<UIItemSkill> onHoverEnter, Action<UIItemSkill> onHoverExit)
    {
        Data = data;
        this.onHoverEnter = onHoverEnter;
        this.onHoverExit = onHoverExit;
    }

    public override void Refresh()
    {
        icon.SetSprite(Data.icon);
        txtName.text = Data.name;
    }

    public void OnHoverEnter()
    {
        onHoverEnter(this);
    }

    public void OnHoverExit()
    {
        onHoverExit(this);
    }
}
