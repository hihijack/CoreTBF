using UnityEngine;
using System.Collections;
using UnityEngine.Playables;

[System.Serializable]
public class AssetCreateEff : PlayableAsset
{
    public string effName;
    public ETargetType target;
    public Vector2 posOffset;
    public bool bind;
    public bool useClipDur;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        //创建一个基于PlayAnimPlayableB的scriptPlayable
        var scriptPlayable = ScriptPlayable<BehavCreateEff>.Create(graph);
        //取脚本实例
        var playable = scriptPlayable.GetBehaviour();
        //设置实例参数
        playable.param = new PBParamCreateEff() { effName = effName, target = target, posOffset = posOffset, useClipDur = useClipDur, bind = bind};
        return scriptPlayable;
    }
}
