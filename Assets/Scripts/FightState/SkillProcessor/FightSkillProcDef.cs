using DefaultNamespace;
using SimpleJSON;
using System;
using System.Collections.Generic;

public class FightSkillProcDef : FightSkillProcessorBase
{
    public float phy;

    public FightSkillProcDef(FightActionBase fightAction, JSONNode jsonData, FightSkillConditionBase condition) : base(fightAction, jsonData, condition)
    {
        
    }

    public override void Proc()
    {
        fightAction.caster.State = ECharacterState.Def;
        //韧性改变至指定百分比;0不改变
        if (fightAction.skill.tenChangeTo > 0)
        {
            fightAction.caster.propData.SetTenacityPercent(fightAction.skill.tenChangeTo);
        }

        UIHPRoot.Inst.RefreshTarget(fightAction.caster);
        fightAction.caster.mTimeStiff = fightAction.skill.backswing;
    }

    protected override void ParseFrom(JSONNode jsonData)
    {
        phy = jsonData["phy"].AsFloat;
    }
}
