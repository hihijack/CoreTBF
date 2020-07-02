using UnityEngine;
using System.Collections;
using UnityEngine.Playables;

[System.Serializable]
public class AssetSetCharacterSprite : PlayableAsset
{

    public ETargetType target;
    public string spriteName;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        //创建一个基于PlayAnimPlayableB的scriptPlayable
        var scriptPlayable = ScriptPlayable<BehavSetCharacterSprite>.Create(graph);
        //取脚本实例
        var playable = scriptPlayable.GetBehaviour();
        //设置实例参数
        playable.param = new PBParamSetCharacterSprite() { target = target, spriteName = spriteName };
        return scriptPlayable;
    }
}
