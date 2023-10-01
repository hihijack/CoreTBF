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
        List<SkillProcResultNode> lstProcResult = new List<SkillProcResultNode>();
        foreach (var target in targets)
        {
            bool isHit = IsHitTarget(content, target);
            if (isHit)
            {
                target.RemoveBuff(targetBuffID);
            }
            lstProcResult.Add(new SkillProcResultNode() { target = target, isHit = isHit });
        }
        return new SkillProcResult() { results = lstProcResult };
    }

    protected override void ParseFrom(JSONNode jsonData)
    {
        targetBuffID = jsonData["buff_id"].AsInt;
    }
}
