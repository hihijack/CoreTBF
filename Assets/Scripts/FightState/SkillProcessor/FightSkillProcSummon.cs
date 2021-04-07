using SimpleJSON;
using System;
using System.Collections.Generic;
using UI;

/// <summary>
/// 召唤
/// </summary>
public class FightSkillProcSummon : FightSkillProcessorBase
{
    int[] roleIds;
    bool isAhead;
    public FightSkillProcSummon(FightActionBase fightAction, JSONNode jsonData, FightSkillConditionBase condition) : base(fightAction, jsonData, condition)
    {

    }

    public override void Proc()
    {
        foreach (var roleId in roleIds)
        {
            var character = FightState.Inst.characterMgr.AddCharacter(roleId, fightAction.caster.camp, isAhead);
            if (character != null)
            {
                //血条
                UIMgr.Inst.GetUI<UIHPRoot>(UITable.EUITable.UIHPRoot).RefreshTarget(character);
                UIMgr.Inst.GetUI<UIFight>(UITable.EUITable.UIFight).RefreshCharacter(character);
            }
            else
            {
                break;
            }
        }
        FightState.Inst.characterMgr.RefreshAllUnitPos(fightAction.caster.camp);
        UIMgr.Inst.GetUI<UIFight>(UITable.EUITable.UIFight).RefreshAIItems();
    }

    protected override void ParseFrom(JSONNode jsonData)
    {
        var roles = jsonData["role_ids"];
        roleIds = new int[roles.Count];
        for (int i = 0; i < roles.Count; i++)
        {
            roleIds[i] = roles[i].AsInt;
        }
           
        isAhead = jsonData["ahead"].AsBool;
    }
}
