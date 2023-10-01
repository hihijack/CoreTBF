using System;
using System.Collections.Generic;

public class FightEventChangePos : FightEventBase
{
    public Character target;
    public bool withAnim;

    public FightEventChangePos(Character target, bool withAnim)
    {
        this.target = target;
        this.withAnim = withAnim;
    }

    internal override FightViewCmdBase ParseToViewCmd()
    {
        return new FightViewCmdRefChrPos(target, withAnim);
    }
}
