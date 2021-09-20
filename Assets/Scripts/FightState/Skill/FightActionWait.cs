using Data;
using System.Collections.Generic;

public class FightActionWait : FightActionBase
{
    public FightActionWait(Skill skill, ActionContent content) : base(skill, content)
    {

    }

    public override void Act()
    {
        base.Act();
    }

    public override void RealAct()
    {
        base.RealAct();
        var caster = actionContent.caster;
        caster.State = ECharacterState.Wait;
    }
}