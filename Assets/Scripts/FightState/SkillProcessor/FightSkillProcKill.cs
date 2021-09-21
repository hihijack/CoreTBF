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
        foreach (var target in targets)
        {
            var oriHP = target.propData.hp;
            target.Killed();
            FightState.Inst.fightViewBehav.CacheViewCmd(new FightViewCmdHPChanged(target, oriHP, 0));
            target.HandleHPState(content);
        }
        return new SkillProcResult() { targets = targets };
    }

    protected override void ParseFrom(JSONNode jsonData)
    {
    }
}
