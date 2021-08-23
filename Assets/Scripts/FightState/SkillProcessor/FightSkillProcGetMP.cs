using DefaultNamespace;
using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

/// <summary>
/// 改变能量
/// </summary>
public class FightSkillProcGetMP : FightSkillProcessorBase
{
    public string targetType;
    public int val;

    public FightSkillProcGetMP(ISkillProcOwner owner, JSONNode jsonData, FightSkillConditionBase condition) : base(owner, jsonData, condition)
    {
    }

    public override List<Character> GetTargets(ActionContent content)
    {
        return null;
    }

    public override SkillProcResult Proc(ActionContent content)
    {
        PlayerRolePropDataMgr.Inst.ChangeMP(val);
        FightState.Inst.fightViewBehav.CacheViewCmd(new FightViewCmdMPChange(0, PlayerRolePropDataMgr.Inst.propData.mp));
        return new SkillProcResult() { targets = null };
    }

    protected override void ParseFrom(JSONNode jsonData)
    {
        targetType = jsonData["target"];
        val = jsonData["val"].AsInt;
    }
}
