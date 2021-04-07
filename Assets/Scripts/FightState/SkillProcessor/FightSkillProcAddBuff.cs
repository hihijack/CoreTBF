using DefaultNamespace;
using SimpleJSON;
using System;
using System.Collections.Generic;

public class FightSkillProcAddBuff : FightSkillProcessorBase
{
    public int buffTID;
    public float dur;
    public string targetType;

    public FightSkillProcAddBuff(FightActionBase fightAction, JSONNode jsonData, FightSkillConditionBase condition) : base(fightAction, jsonData, condition)
    {

    }

    public override void Proc()
    {
        var targets = fightAction.targets;
        var caster = fightAction.caster;
        var skill = fightAction.skill;

        List<Character> lstTarget = null;

        switch (targetType)
        {
            case SkillProcTarget.Targets:
                lstTarget = targets;
                break;
            case SkillProcTarget.Self:
                lstTarget = new List<Character>();
                lstTarget.Add(caster);
                break;
            case SkillProcTarget.RanTarget:
                //TODO 随机目标
                break;
        }

        if (lstTarget != null)
        {
            foreach (var target in lstTarget)
            {
                target.AddABuff(buffTID, dur, caster);
            }
        }
    }

    protected override void ParseFrom(JSONNode jsonData)
    {
        buffTID = jsonData["buff_id"].AsInt;
        dur = jsonData["dur"].AsFloat;
        targetType = jsonData["target"];
    }
}
