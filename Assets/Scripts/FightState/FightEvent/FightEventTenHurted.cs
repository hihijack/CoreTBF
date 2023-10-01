using System;
using System.Collections.Generic;

public class FightEventTenHurted : FightEventBase
{
    public Character target;
    public int tenOri;
    public int tenChange;

    public FightEventTenHurted(Character target, int tenOri, int tenChange)
    {
        this.target = target;
        this.tenOri = tenOri;
        this.tenChange = tenChange;
    }

    internal override FightViewCmdBase ParseToViewCmd()
    {
        return new FightViewCmdTenacityChange(target, tenOri, tenOri + tenChange);
    }
}
