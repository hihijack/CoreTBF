using Data;
using System.Collections.Generic;
using UI;

public class FightActionDef : FightActionBase
{
    public FightActionDef(Skill skill, ActionContent content) : base(skill, content)
    {

    }

    public override void Act()
    {
        base.Act();
    }

    public override void RealAct()
    {
        base.RealAct();
        this.ProcActEffect();
    }
}