using System;
using System.Collections.Generic;

public class FightEventDie : FightEventBase
{
    public Character target;

    public FightEventDie(Character target)
    {
        this.target = target;
    }

    internal override FightViewCmdBase ParseToViewCmd()
    {
        return new FightViewCmdCharacterDie(target);
    }
}
