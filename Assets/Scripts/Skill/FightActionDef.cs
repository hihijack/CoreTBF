namespace DefaultNamespace
{
    public class FightActionDef : FightActionBase
    {
        public override void Act()
        {
            base.Act();
            caster.State = ECharacterState.Def;
            actionEnd();
        }
    }
}