using SimpleJSON;
using System;
using System.Collections.Generic;

/// <summary>
/// 直接杀死目标：hp直接归0，不触发受伤被动
/// </summary>
public class FightSkillProcKill : FightSkillProcessorBase
{
    public FightSkillProcKill(ISkillProcOwner owner, JSONNode jsonData, FightSkillConditionBase condition) : base(owner, jsonData, condition)
    {
    }

    public override SkillProcResult Proc(ActionContent content)
    {
        var targets = GetTargets(content);
        List<SkillProcResultNode> lstProcResult = new List<SkillProcResultNode>();
        foreach (var target in targets)
        {
            var isHit = IsHitTarget(content, target);
            if (isHit)
            {
                FightState.Inst.eventRecorder.CacheEvent(new FightEventDie(target));
                var oriHP = target.propData.hp;
                target.Killed();
                target.HandleHPState(content);
            }
            lstProcResult.Add(new SkillProcResultNode() { target = target, isHit = isHit });
        }
        return new SkillProcResult() { results = lstProcResult };
    }

    protected override void ParseFrom(JSONNode jsonData)
    {
    }
}
