using System;
using System.Collections.Generic;

public class TargetFinderEnemyTank : SkillProcTargetFinderBase
{
    private readonly ISkillProcOwner owner;

    public TargetFinderEnemyTank(ISkillProcOwner owner)
    {
        this.owner = owner;
    }

    public List<Character> GetTargets(ActionContent content)
    {
        //敌方最前排目标
        var targets = new List<Character>();
        targets.Add(FightState.Inst.characterMgr.GetTankCharacter(owner.GetOwnerCharacter().GetEnemyCamp()));
        return targets;
    }
}
