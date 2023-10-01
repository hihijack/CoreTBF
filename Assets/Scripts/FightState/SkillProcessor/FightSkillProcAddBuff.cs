using DefaultNamespace;
using SimpleJSON;
using System;
using System.Collections.Generic;

public class FightSkillProcAddBuff : FightSkillProcessorBase
{
    public int buffTID;
    public float dur;
    public string targetType;

    public FightSkillProcAddBuff(ISkillProcOwner owner, JSONNode jsonData, FightSkillConditionBase condition) : base(owner, jsonData, condition)
    {

    }

    public override SkillProcResult Proc(ActionContent content)
    {
        var selfCharacter = owner.GetOwnerCharacter();
        var lstTarget = GetTargets(content);
        List<SkillProcResultNode> lstProcResult = new List<SkillProcResultNode>();
        if (lstTarget != null)
        {
            foreach (var target in lstTarget)
            {
                bool isHit = IsHitTarget(content, target);
                if (isHit)
                {
                    target.AddABuff(buffTID, dur, selfCharacter);
                }
                lstProcResult.Add(new SkillProcResultNode() { target = target, isHit = isHit });
            }
        }
        return new SkillProcResult() { results = lstProcResult};
    }

    protected override void ParseFrom(JSONNode jsonData)
    {
        buffTID = jsonData["buff_id"].AsInt;
        dur = jsonData["dur"].AsFloat;
        targetType = jsonData["target"];
    }
}
