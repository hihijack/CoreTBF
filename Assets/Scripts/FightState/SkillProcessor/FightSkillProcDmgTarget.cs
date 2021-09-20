using DefaultNamespace;
using SimpleJSON;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FightSkillProcDmgTarget : FightSkillProcessorBase
{
    public FightSkillProcDmgTarget(ISkillProcOwner owner, JSONNode jsonData, FightSkillConditionBase condition) : base(owner, jsonData, condition)
    {

    }

    public override SkillProcResult Proc(ActionContent content)
    {
        List<Character> targets = GetTargets(content);
        var selfCharacter = owner.GetOwnerCharacter();
        var skill = owner.GetOwnerSkill();
        var skillBaseData = skill.GetBaseData();
        foreach (var target in targets)
        {

            var oriHP = target.propData.hp;
            var oriTenacity = target.propData.tenacity;
            var result = selfCharacter.DamageTarget(target, new DmgData() { dmgPrecent = skillBaseData.dmg, timeAtkStiff = skillBaseData.timeAtkStiff, tenAtk = skillBaseData.dmgTenacity });

            var curHP = target.propData.hp;
            var curTenacity = target.propData.tenacity;

            FightState.Inst.fightViewBehav.CacheViewCmd(new FightViewCmdHPChanged(target, oriHP, curHP));
            FightState.Inst.fightViewBehav.CacheViewCmd(new FightViewCmdTenacityChange(target, oriTenacity, curTenacity));

            Debug.Log($"Damage Prco:{selfCharacter.roleData.name}->{target.roleData.name}:{result.dmg}");//#########

            if (result.dmg > 0 && target.IsAlive())
            {
                target.OnHurtd(content);
            }
            target.HandleHPState(content);
        }
        return new SkillProcResult() { targets = targets };
    }

    protected override void ParseFrom(JSONNode jsonData)
    {

    }
}
