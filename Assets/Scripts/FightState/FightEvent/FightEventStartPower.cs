using System;
using System.Collections.Generic;

/// <summary>
/// 开始蓄力
/// </summary>
public class FightEventStartPower : FightEventBase
{
    public Character caster;
    public Skill skill;

    public FightEventStartPower(Character caster, Skill skill)
    {
        this.caster = caster;
        this.skill = skill;
    }

    internal override FightViewCmdBase ParseToViewCmd()
    {
        return new FightViewCmdCastSkill(caster, null, skill, true);
    }
}
