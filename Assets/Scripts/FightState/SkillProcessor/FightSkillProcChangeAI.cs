using DefaultNamespace;
using SimpleJSON;
using System;
using System.Collections.Generic;

/// <summary>
/// 改变AI
/// </summary>
public class FightSkillProcChangeAI : FightSkillActiveableProcBase
{
    int[] arrAI;
    public FightSkillProcChangeAI(ISkillProcOwner owner, JSONNode jsonData, FightSkillConditionBase condition) : base(owner, jsonData, condition)
    {
    }

    public override List<Character> GetTargets(ActionContent content)
    {
        return null;
    }

    public override SkillProcResult Proc(ActionContent content)
    {
        var selfCharacter = owner.GetOwnerCharacter();
        selfCharacter.ai.SetExrAI(arrAI);
        return new SkillProcResult();
    }

    public override SkillProcResult UnProc(ActionContent content)
    {
        var selfCharacter = owner.GetOwnerCharacter();
        selfCharacter.ai.RemoveExtAI();
        return new SkillProcResult();
    }

    protected override void ParseFrom(JSONNode jsonData)
    {
        arrAI = jsonData["ais"].AsArray.ToIntArr();
    }
}
