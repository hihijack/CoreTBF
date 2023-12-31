﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UI;

public class UIItemBuff : MonoBehaviour
{
    public Image icon;
    public Image mask;
    public Text txtDur;
    public Text txtLayer;

    public BuffBase data;

    public void SetTarget(BuffBase buff)
    {
        data = buff;
    }

    public void Refresh()
    {
        //icon
        GameUtil.SetSprite(icon, "Sprites/Buffs" , data.GetBuffData().icon);
        //dur
        var durLeft = data.GetDurLeft();
        txtDur.text = durLeft > 0 ? durLeft.ToString("0.0") : "N/A";
        //mask
        var prog = data.GetDurProg();
        mask.fillAmount = prog > 0 ? prog : 0;
        //layer
        if (data.GetLayer() > 1)
        {
            txtLayer.text = data.GetLayer().ToString();
        }
        else
        {
            txtLayer.text = "";
        }
    }

    public void OnHoverIn()
    {
        if (data != null)
        {
            var uiTip = UIMgr.Inst.ShowUI(UITable.EUITable.UITip) as UITip;
            uiTip.Refresh(data.GetBuffData().desc);
        }
    }

    public void OnHoverOut()
    {
        UIMgr.Inst.HideUI(UITable.EUITable.UITip);
    }
}
