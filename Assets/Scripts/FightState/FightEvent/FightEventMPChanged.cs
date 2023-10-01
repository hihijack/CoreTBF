using System;
using System.Collections.Generic;

public class FightEventMPChanged : FightEventBase
{
    public int oriVal;
    public int changeVal;

    public FightEventMPChanged(int oriVal, int changeVal)
    {
        this.oriVal = oriVal;
        this.changeVal = changeVal;
    }

    internal override FightViewCmdBase ParseToViewCmd()
    {
        return new FightViewCmdMPChange(oriVal, oriVal + changeVal);
    }
}
