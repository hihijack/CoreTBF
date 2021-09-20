using DefaultNamespace;
using SimpleJSON;
using System;
using System.Collections.Generic;

/// <summary>
/// 移除buff
/// </summary>
public class FightSkillProcRemoveBuff : FightSkillProcessorBase
{
    int targetBuffID;
    public FightSkillProcRemoveBuff(ISkillProcOwner owner, JSONNode jsonData, FightSkillConditionBase condition) : base(owner, jsonData, condition)
    {
    }

    public override SkillProcResult Proc(ActionContent content)
    {
        var targets = GetTargets(content);
        foreach (var target in targets)
        {
            target.RemoveBuff(targetBuffID);
        }
        return new SkillProcResult() { targets = targets };
    }

    protected override void ParseFrom(JSONNode jsonData)
    {
        targetBuffID = jsonData["buff_id"].AsInt;
    }
}
