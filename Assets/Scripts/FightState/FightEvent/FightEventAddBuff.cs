using System;
using System.Collections.Generic;

/// <summary>
/// 添加buff
/// </summary>
public class FightEventAddBuff : FightEventBase
{
    public Character target;
    public BuffBase buff;

    internal override FightViewCmdBase ParseToViewCmd()
    {
        return null;
    }
}
