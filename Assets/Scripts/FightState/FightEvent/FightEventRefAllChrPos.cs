using System;
using System.Collections.Generic;

public class FightEventRefAllChrPos : FightEventBase
{
    public ECamp camp;
    public bool withAnim;

    public FightEventRefAllChrPos(ECamp camp, bool withAnim)
    {
        this.camp = camp;
        this.withAnim = withAnim;
    }

    internal override FightViewCmdBase ParseToViewCmd()
    {
        return new FightViewCmdRefAllChrPos(camp, withAnim);
    }
}
