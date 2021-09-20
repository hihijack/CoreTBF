using SimpleJSON;
using System;
using System.Collections.Generic;

public class TargetFinderRoleID : SkillProcTargetFinderBase
{
    int roleID;

    public TargetFinderRoleID(int roleID)
    {
        this.roleID = roleID;
    }

    public TargetFinderRoleID(JSONNode jsonNode)
    {
        this.roleID = jsonNode["target_role"].AsInt;
    }

    public List<Character> GetTargets(ActionContent content)
    {
        var lst = new List<Character>();
        var target = FightState.Inst.characterMgr.GetCharacterByID(roleID);
        lst.Add(target);
        return lst;
    }
}
