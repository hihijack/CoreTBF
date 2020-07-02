using UnityEngine;
using System.Collections;
using UnityEngine.Playables;

public enum EEventLogic
{
    End,
    RemoveEffects,
    RealAct
}

public class BehavEventLogic : PlayableBehaviour
{
    public EEventLogic eventType;

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        base.OnBehaviourPlay(playable, info);
        GameMgr.Inst.fightViewBehav.OnPlayableEventLogic(eventType);
    }
}
