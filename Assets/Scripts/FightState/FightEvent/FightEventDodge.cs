using System;
using System.Collections.Generic;

/// <summary>
/// 闪避
/// </summary>
public class FightEventDodge : FightEventBase
{
    public Character target;

    public FightEventDodge(Character target)
    {
        this.target = target;
    }
}
