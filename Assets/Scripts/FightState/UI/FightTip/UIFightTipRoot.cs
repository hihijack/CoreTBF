using UnityEngine;
using System.Collections;
using System;

public class UIFightTipRoot : UIBase
{
    public GameObject pfbDmgItemTip;

    public static UIFightTipRoot Inst { get; private set; }

    protected override void OnAwake()
    {
        base.OnAwake();
        Inst = this;
    }

    public void ShowDmgTip(Character target, DmgResult dmg)
    {
        Debug.Log("t>>ShowDmgTip:" + dmg.dmg);//####
        var dmgTip = UIItemFightDmg.Create<UIItemFightDmg>(transform, pfbDmgItemTip);
        dmgTip.SetData(target, dmg);
        dmgTip.Refresh();
    }

    internal void ShowHealTip(Character target, HealResult healResult)
    {
        var dmgTip = UIItemFightDmg.Create<UIItemFightDmg>(transform, pfbDmgItemTip);
        dmgTip.SetData(target, healResult);
        dmgTip.Refresh();
    }
}
