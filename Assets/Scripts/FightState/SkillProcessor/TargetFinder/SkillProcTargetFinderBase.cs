using System.Collections.Generic;

public interface SkillProcTargetFinderBase
{
    List<Character> GetTargets(ActionContent content);
}
