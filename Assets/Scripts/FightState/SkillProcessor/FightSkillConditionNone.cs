using DefaultNamespace;
using SimpleJSON;
using System;
using System.Collections.Generic;
/// <summary>
/// 无条件
/// </summary>
public class FightSkillConditionNone : FightSkillConditionBase
{
    public FightSkillConditionNone(FightActionBase fightAction, JSONNode jsonData) : base(fightAction,  jsonData)
    {
    }
    public override bool IsTrue()
    {
        return true;
    }

    protected override void ParseFrom(JSONNode jsonData)
    {
        
    }
}
