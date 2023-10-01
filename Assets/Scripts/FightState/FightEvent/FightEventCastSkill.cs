using System;
using System.Collections.Generic;

public class FightEventCastSkill : FightEventBase
{
    public Character caster;
    public Skill skill;
    public List<Character> targets;

    public FightEventCastSkill(Character caster, Skill skill, List<Character> targets)
    {
        this.caster = caster;
        this.skill = skill;
        this.targets = targets;
    }

    internal override FightViewCmdBase ParseToViewCmd()
    {
        return new FightViewCmdCastSkill(caster, targets, skill, false);
    }
}
