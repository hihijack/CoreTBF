using System;
using System.Collections.Generic;

public class FightViewCmdHurted : FightViewCmdBase
{
    private readonly Character target;
    private readonly int oriVal;
    private readonly int curVal;
    DmgResult dmgResult;

    public FightViewCmdHurted(Character target, int oriVal, int curVal, DmgResult dmgResult)
    {
        this.target = target;
        this.oriVal = oriVal;
        this.curVal = curVal;
        this.dmgResult = dmgResult;
    }

    public override void Play()
    {
        base.Play();
        UIHPRoot.Inst.RefreshTargetHPWithVal(target, curVal);
        //显示飘字
        UIFightTipRoot.Inst.ShowDmgTip(target, dmgResult);
        End();
    }
}
