using DefaultNamespace.FightStages;
using UnityEngine;

public class FightStageNormalView : FightStageBase
{
    public FightStageNormalView()
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log($"t[{Time.frameCount}]>>进入表现阶段");//##########
        FightState.Inst.fightViewBehav.StartPlayCachedViewCmd(OnViewPlayEnd);
    }

    private void OnViewPlayEnd()
    {
        Debug.Log($"t[{Time.frameCount}]>>离开表现阶段");//##########
        FightState.Inst.SetFightStage(EFightStage.Normal);
    }
}
