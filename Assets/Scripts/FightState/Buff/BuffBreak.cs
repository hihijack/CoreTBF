using UnityEngine;
using System.Collections;
using DefaultNamespace;

public class BuffBreak : BuffBase
{
    float paramMul;
    public BuffBreak(BuffBaseData data, Character target, Character caster, int layer, float dur) : base(data, target, caster, layer, dur)
    {
        paramMul = data.data["val"].AsFloat;
    }

    public override void OnAdd()
    {
        base.OnAdd();
        //指定防御类型参数A增加add值,参数B增加mul值
        target.propData.dmgHurtedMul *= paramMul;
    }

    protected override void OnRemoved()
    {
        base.OnRemoved();
        target.propData.dmgHurtedMul /= paramMul;
    }
}
