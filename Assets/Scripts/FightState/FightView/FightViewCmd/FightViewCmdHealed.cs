using System;
using System.Collections.Generic;

public class FightViewCmdHealed : FightViewCmdBase
{
    private readonly Character target;
    private readonly int oriVal;
    private readonly int curVal;
    private readonly HealResult healResult;

    public FightViewCmdHealed(Character target, int oriVal, int curVal, HealResult healResult)
    {
        this.target = target;
        this.oriVal = oriVal;
        this.curVal = curVal;
        this.healResult = healResult;
    }

    public override void Play()
    {
        base.Play();
        UIHPRoot.Inst.RefreshTargetHPWithVal(target, curVal);
        //显示飘字
        UIFightTipRoot.Inst.ShowHealTip(target, healResult);
        End();
    }
}
