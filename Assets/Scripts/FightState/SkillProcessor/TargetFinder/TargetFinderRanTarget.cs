using System;
using System.Collections.Generic;

public class TargetFinderRanTarget : SkillProcTargetFinderBase
{

    private readonly ISkillProcOwner owner;

    public TargetFinderRanTarget(ISkillProcOwner owner)
    {
        this.owner = owner;
    }

    public List<Character> GetTargets(ActionContent content)
    {
        return FightState.Inst.characterMgr.GetRandomOfCamp(1, owner.GetOwnerCharacter().GetEnemyCamp());
    }
}
