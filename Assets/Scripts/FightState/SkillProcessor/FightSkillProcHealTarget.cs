using DefaultNamespace;
using SimpleJSON;
using System;
using System.Collections.Generic;

/// <summary>
/// 治疗目标
/// </summary>
public class FightSkillProcHealTarget : FightSkillProcessorBase
{
    public string targetType;

    public FightSkillProcHealTarget(ISkillProcOwner owner, JSONNode jsonData, FightSkillConditionBase condition) : base(owner, jsonData, condition)
    {
    }

    public override List<Character> GetTargets(ActionContent content)
    {
        return base.GetTargets(targetType, content);
    }

    public override SkillProcResult Proc(ActionContent content)
    {
        List<Character> targets = GetTargets(content);
        var selfCharacter = owner.GetOwnerCharacter();
        var skill = owner.GetOwnerSkill();
        var skillBaseData = skill.GetBaseData();
        foreach (var target in targets) 
        {
            int oriHP = target.propData.hp;
            selfCharacter.HealTarget(target, skillBaseData.dmg);
            FightState.Inst.fightViewBehav.CacheViewCmd(new FightViewCmdHPChanged(target, oriHP, target.propData.hp));
            target.HandleHPState(content);
        }
        return new SkillProcResult() { targets = targets };
    }

    protected override void ParseFrom(JSONNode jsonData)
    {
        targetType = jsonData["target"];
    }
}
