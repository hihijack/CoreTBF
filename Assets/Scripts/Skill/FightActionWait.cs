namespace DefaultNamespace
{
    public class FightActionWait : FightActionBase
    {
        public override void Act()
        {
            base.Act();
            caster.State = ECharacterState.Wait;
            actionEnd();
        }
    }
}