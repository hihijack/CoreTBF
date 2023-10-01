using DefaultNamespace;
using SimpleJSON;
using System;
using System.Collections.Generic;

public class FightSkillProcChangeLoc : FightSkillProcessorBase
{
    string locChangeType;
    public FightSkillProcChangeLoc(ISkillProcOwner owner, JSONNode jsonData, FightSkillConditionBase condition) : base(owner, jsonData, condition)
    {
    }

    public override SkillProcResult Proc(ActionContent content)
    {
        var targets = GetTargets(content);
        var target = targets[0];

        var isHit = IsHitTarget(content, target);
        if (isHit)
        {
            int loc = 0;
            if (locChangeType == LocChangeType.AHEAD)
            {
                loc = 1;
            }
            else if (locChangeType == LocChangeType.LAST)
            {
                loc = FightState.Inst.characterMgr.GetCharactersCount(target.camp);
            }

            var hasChange = FightState.Inst.characterMgr.ChangeToLoc(target, loc);
            if (hasChange)
            {
                FightState.Inst.eventRecorder.CacheEvent(new FightEventRefAllChrPos(target.camp, true));
            }
        }
        else
        {
            FightState.Inst.eventRecorder.CacheEvent(new FightEventDodge(target));
        }

       
        return new SkillProcResult() { };
    }

    protected override void ParseFrom(JSONNode jsonData)
    {
        locChangeType = jsonData["loc"];
    }
}
