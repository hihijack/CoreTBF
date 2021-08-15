using DefaultNamespace;
using SimpleJSON;
using System;
using System.Collections.Generic;

public class FightSkillProcDef : FightSkillProcessorBase
{
    public float phy;

    public FightSkillProcDef(ISkillProcOwner owner, JSONNode jsonData, FightSkillConditionBase condition) : base(owner, jsonData, condition)
    {
        
    }

    public override List<Character> GetTargets(ActionContent content)
    {
        return null;
    }

    public override SkillProcResult Proc(ActionContent content)
    {
        content.caster.State = ECharacterState.Def;
        var skillBaseData = content.skill.GetBaseData();
        //韧性改变至指定百分比;0不改变
        if (skillBaseData.tenChangeTo > 0)
        {
            content.caster.propData.SetTenacityPercent(skillBaseData.tenChangeTo);
        }

        UIHPRoot.Inst.RefreshTarget(content.caster);
        content.caster.mTimeStiff = skillBaseData.backswing;
        return new SkillProcResult();
    }

    protected override void ParseFrom(JSONNode jsonData)
    {
        phy = jsonData["phy"].AsFloat;
    }
}
