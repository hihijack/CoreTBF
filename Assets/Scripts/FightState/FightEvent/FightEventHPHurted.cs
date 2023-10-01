using System;
using System.Collections.Generic;

public class FightEventHPHurted : FightEventBase
{
    public Character target;
    public int hpOri;
    public int hpCur;
    public DmgResult dmgResult;

    public FightEventHPHurted(Character target, int hpOri, int hpCur, DmgResult dmgResult)
    {
        this.target = target;
        this.hpOri = hpOri;
        this.hpCur = hpCur;
        this.dmgResult = dmgResult;
    }

    internal override FightViewCmdBase ParseToViewCmd()
    {
        return new FightViewCmdHurted(target, hpOri, hpCur, dmgResult);
    }
}
