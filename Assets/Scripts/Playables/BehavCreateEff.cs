using UnityEngine;
using UnityEngine.Playables;

public struct PBParamCreateEff
{
    public string effName;
    public ETargetType target;
    public Vector2 posOffset;
    public bool bind;
    public bool useClipDur;
    public float dur;
}

public class BehavCreateEff : PlayableBehaviour
{
    public PBParamCreateEff param;

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        base.OnBehaviourPlay(playable, info);
        FightState.Inst.fightViewBehav.OnPlayableCreateEff(param);
    }
}
