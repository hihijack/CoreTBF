namespace DefaultNamespace
{
    public class FightActionWait : FightActionBase
    {
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