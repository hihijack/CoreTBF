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
            //韧性改变至指定百分比;0不改变
            if (skill.tenChangeTo > 0)
            {
                caster.propData.SetTenacityPercent(skill.tenChangeTo);
            }

            UIHPRoot.Inst.RefreshTarget(caster);
            caster.mTimeStiff = skill.backswing;
        }
    }
}