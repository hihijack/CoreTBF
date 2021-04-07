using DefaultNamespace;
using SimpleJSON;
using System;
using System.Collections.Generic;

public class FightSkillProcDmgTarget : FightSkillProcessorBase
{
    public FightSkillProcDmgTarget(FightActionBase fightAction, JSONNode jsonData, FightSkillConditionBase condition) : base(fightAction, jsonData, condition)
    {

    }

    public override void Proc()
    {
        var targets = fightAction.targets;
        var caster = fightAction.caster;
        var skill = fightAction.skill;
        foreach (var target in targets)
        {
            caster.DamageTarget(target, new DmgData() { dmgPrecent = skill.dmg, timeAtkStiff = skill.timeAtkStiff, tenAtk = skill.dmgTenacity });
        }
    }

    protected override void ParseFrom(JSONNode jsonData)
    {
       
    }
}
