namespace DefaultNamespace.FightStages
{
    public class FightStageBase
    {
        public virtual void OnEnter(){}
        public virtual void OnUpdate(){}
        public virtual void OnExit(){}
        
        public virtual void SetFlagIdle(){}
    }
}