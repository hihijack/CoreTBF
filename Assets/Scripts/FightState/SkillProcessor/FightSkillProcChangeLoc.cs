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
            FightState.Inst.fightViewBehav.CacheViewCmd(new FightViewCmdRefAllChrPos(target.camp, true));
        }
        return new SkillProcResult() { targets = targets };
    }

    protected override void ParseFrom(JSONNode jsonData)
    {
        locChangeType = jsonData["loc"];
    }
}
