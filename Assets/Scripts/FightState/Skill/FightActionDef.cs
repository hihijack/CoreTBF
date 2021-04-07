using Data;
using System.Collections.Generic;
using UI;

namespace DefaultNamespace
{
    public class FightActionDef : FightActionBase
    {
        public FightActionDef(Character caster, SkillBaseData skill, List<Character> targets) : base(caster, skill, targets)
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
}