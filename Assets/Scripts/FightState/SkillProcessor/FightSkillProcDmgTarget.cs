using DefaultNamespace;
using SimpleJSON;
using System;
using System.Collections.Generic;

public class FightSkillProcDmgTarget : FightSkillProcessorBase
{
    public string targetType;
    public FightSkillProcDmgTarget(ISkillProcOwner owner, JSONNode jsonData, FightSkillConditionBase condition) : base(owner, jsonData, condition)
    {

    }

    public override List<Character> GetTargets(ActionContent content)
    {
        if (m_cacheTargets != null)
        {
            return m_cacheTargets;
        }

        List<Character> targets = null;
        var selfCharacter = owner.GetOwnerCharacter();
        switch (targetType)
        {
            case SkillProcTarget.Targets:
                targets = content.targets;
                break;
            case SkillProcTarget.Self:
                targets = new List<Character>();
                targets.Add(selfCharacter);
                break;
            case SkillProcTarget.RanTarget:
                targets = FightState.Inst.characterMgr.GetRandomOfCamp(1, selfCharacter.GetEnemyCamp());
                break;
            default:
                break;
        }

        return targets;
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
        targetType = jsonData["target"];
    }
}
