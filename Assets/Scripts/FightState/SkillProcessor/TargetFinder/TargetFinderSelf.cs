using System;
using System.Collections.Generic;

public class TargetFinderSelf : SkillProcTargetFinderBase
{
    private readonly ISkillProcOwner owner;

    public TargetFinderSelf(ISkillProcOwner owner)
    {
        this.owner = owner;
    }

    public List<Character> GetTargets(ActionContent content)
    {
        var targets = new List<Character>();
        targets.Add(owner.GetOwnerCharacter());
        return targets;
    }
}
