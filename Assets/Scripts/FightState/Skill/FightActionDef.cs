using UI;

namespace DefaultNamespace
{
    public class FightActionDef : FightActionBase
    {
        public override void Act()
        {
            base.Act();
        }

        public override void RealAct()
        {
            base.RealAct();
            caster.State = ECharacterState.Def;
            //增加护盾
            //caster.propData.ChangeShield(1);
        }
    }
}