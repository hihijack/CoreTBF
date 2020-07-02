using UnityEngine;
using System.Collections;
using UnityEngine.Playables;

public enum ETargetType
{
    Atker,
    Targets
}

public enum EPointType
{
    OriPos,
    CloseUp
}

public struct PBParamMoveCharacter
{
    public ETargetType targetType;
    public EPointType pointType;
    public Vector3 offsetPos;
    public bool atonce;
    public float dur;
}

public class BehavMoveCharacter : PlayableBehaviour
{
    public PBParamMoveCharacter param;

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        base.OnBehaviourPlay(playable, info);
        param.dur = (float)playable.GetDuration();
        GameMgr.Inst.fightViewBehav.OnPlayableMoveCharacter(param);
    }
}
