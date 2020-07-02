using System;
using UnityEngine.Playables;

public struct PBParamSetCharacterSprite
{
    public ETargetType target;
    public string spriteName; 
}

public class BehavSetCharacterSprite : PlayableBehaviour
{
    public PBParamSetCharacterSprite param;

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        base.OnBehaviourPlay(playable, info);
        GameMgr.Inst.fightViewBehav.OnPlayableSetSprite(param);
    }
}
