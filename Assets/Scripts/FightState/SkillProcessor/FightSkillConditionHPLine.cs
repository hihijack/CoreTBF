using SimpleJSON;
using System;
using System.Collections.Generic;

/// <summary>
/// 血线判断。默认检测自身
/// </summary>
public class FightSkillConditionHPLine : FightSkillConditionBase
{
    float valPercent;
    public FightSkillConditionHPLine(JSONNode jsonData) : base(jsonData)
    {
    }

    public override bool IsTrue()
    {
        var owner = GetOwner();
        return owner.propData.GetHPPercent() <= valPercent;
    }

    protected override void ParseFrom(JSONNode jsonData)
    {
        valPercent = jsonData["val"].AsFloat;
    }
}
