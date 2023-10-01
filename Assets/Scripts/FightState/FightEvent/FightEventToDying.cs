using System;
using System.Collections.Generic;

/// <summary>
/// 濒死
/// </summary>
public class FightEventToDying : FightEventBase
{
    public Character target;
    public int oriHP;

    public FightEventToDying(Character target, int oriHP)
    {
        this.target = target;
        this.oriHP = oriHP;
    }

    internal override FightViewCmdBase ParseToViewCmd()
    {
        return new FightViewCmdCharacterDying(target);
    }
}
