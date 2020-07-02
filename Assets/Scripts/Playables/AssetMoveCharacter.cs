using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class AssetMoveCharacter : PlayableAsset
{
    public ETargetType targetType;
    public EPointType pointType;
    public Vector3 offsetPos;
    public bool atonce;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        //创建一个基于PlayAnimPlayableB的scriptPlayable
        var scriptPlayable = ScriptPlayable<BehavMoveCharacter>.Create(graph);
        //取脚本实例
        var playable = scriptPlayable.GetBehaviour();
        //设置实例参数
        playable.param = new PBParamMoveCharacter() { targetType = targetType, pointType = pointType, offsetPos = offsetPos, atonce = atonce, dur = 0 };
        return scriptPlayable;
    }
}
