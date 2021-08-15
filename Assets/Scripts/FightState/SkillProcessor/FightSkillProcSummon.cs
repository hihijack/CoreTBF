using DefaultNamespace;
using SimpleJSON;
using System.Collections.Generic;
using UI;

/// <summary>
/// 召唤
/// </summary>
public class FightSkillProcSummon : FightSkillProcessorBase
{
    int[] roleIds;
    bool isAhead;
    public FightSkillProcSummon(ISkillProcOwner owner, JSONNode jsonData, FightSkillConditionBase condition) : base(owner, jsonData, condition)
    {

    }

    public override List<Character> GetTargets(ActionContent content)
    {
        return null;
    }

    public override SkillProcResult Proc(ActionContent content)
    {
        foreach (var roleId in roleIds)
        {
            var character = FightState.Inst.characterMgr.AddCharacter(roleId, content.caster.camp, isAhead);
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
        FightState.Inst.characterMgr.RefreshAllUnitPos(content.caster.camp);
        UIMgr.Inst.GetUI<UIFight>(UITable.EUITable.UIFight).RefreshAIItems();
        return new SkillProcResult();
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
