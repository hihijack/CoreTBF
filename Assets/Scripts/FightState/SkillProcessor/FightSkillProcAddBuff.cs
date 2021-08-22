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
            case SkillProcTarget.Tank:
                //敌方最前排目标
                targets = new List<Character>();
                targets.Add(FightState.Inst.characterMgr.GetTankCharacter(selfCharacter.GetEnemyCamp()));
                break;
            default:
                break;
        }
        return targets;
    }

    public override SkillProcResult Proc(ActionContent content)
    {
        var selfCharacter = owner.GetOwnerCharacter();
        var lstTarget = GetTargets(content);
        if (lstTarget != null)
        {
            foreach (var target in lstTarget)
            {
                target.AddABuff(buffTID, dur, selfCharacter);
            }
        }
        return new SkillProcResult() { targets = lstTarget };
    }

    protected override void ParseFrom(JSONNode jsonData)
    {
        buffTID = jsonData["buff_id"].AsInt;
        dur = jsonData["dur"].AsFloat;
        targetType = jsonData["target"];
    }
}
