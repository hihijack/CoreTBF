using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class AssetEventLogic : PlayableAsset
{

    public EEventLogic eventType;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        //创建一个基于PlayAnimPlayableB的scriptPlayable
        var scriptPlayable = ScriptPlayable<BehavEventLogic>.Create(graph);
        //取脚本实例
        var playable = scriptPlayable.GetBehaviour();
        //设置实例参数
        playable.eventType = eventType;
        return scriptPlayable;
    }
}
