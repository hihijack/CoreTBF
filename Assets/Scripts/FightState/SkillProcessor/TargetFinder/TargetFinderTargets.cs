using System;
using System.Collections.Generic;

public class TargetFinderTargets : SkillProcTargetFinderBase
{
    public List<Character> GetTargets(ActionContent content)
    {
        return content.targets;
    }
}
