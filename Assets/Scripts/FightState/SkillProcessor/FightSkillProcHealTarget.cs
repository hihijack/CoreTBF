using DefaultNamespace;
using SimpleJSON;
using System;
using System.Collections.Generic;

/// <summary>
/// 治疗目标
/// </summary>
public class FightSkillProcHealTarget : FightSkillProcessorBase
{
    public FightSkillProcHealTarget(ISkillProcOwner owner, JSONNode jsonData, FightSkillConditionBase condition) : base(owner, jsonData, condition)
    {
    }

    public override SkillProcResult Proc(ActionContent content)
    {
        List<Character> targets = GetTargets(content);
        var selfCharacter = owner.GetOwnerCharacter();
        var skill = owner.GetOwnerSkill();
        var skillBaseData = skill.GetBaseData();
        //List<SkillProcResultNode> lstProcResult = new List<SkillProcResultNode>();
        foreach (var target in targets) 
        {
            bool isHit = IsHitTarget(content, target);
            if (isHit)
            {
                int oriHP = target.propData.hp;
                var result = selfCharacter.HealTarget(target, skillBaseData.dmg);
                FightState.Inst.eventRecorder.CacheEvent(new FightEventHPHeal(target, oriHP, target.propData.hp, result));
                target.HandleHPState(content);
            }
            else
            {
                FightState.Inst.eventRecorder.CacheEvent(new FightEventDodge(target));
            }
            //lstProcResult.Add(new SkillProcResultNode() { target = target, isHit = isHit });
        }
        return new SkillProcResult() {};
    }

    protected override void ParseFrom(JSONNode jsonData)
    {
    }
}
