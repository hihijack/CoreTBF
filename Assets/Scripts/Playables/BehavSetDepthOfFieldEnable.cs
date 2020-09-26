using UnityEngine;
using System.Collections;
using UnityEngine.Playables;

public class BehavSetDepthOfFieldEnable : PlayableBehaviour
{
    public bool enable;

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        base.OnBehaviourPlay(playable, info);
        FightState.Inst.fightViewBehav.OnPlayableSetDepthOfFieldEnable(enable);
    }
}
