using Data;
using System.Collections.Generic;

namespace DefaultNamespace
{
    public class FightActionWait : FightActionBase
    {
        public FightActionWait(Character caster, SkillBaseData skill, List<Character> targets) : base(caster, skill, targets)
        {

        }

        public override void Act()
        {
            base.Act();
        }

        public override void RealAct()
        {
            base.RealAct();
            caster.State = ECharacterState.Wait;
        }
    }
}