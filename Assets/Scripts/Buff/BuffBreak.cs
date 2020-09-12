using UnityEngine;
using System.Collections;
using DefaultNamespace;

public class BuffBreak : BuffBase
{
    float paramMul;
    public BuffBreak(BuffData data, Character target, Character caster, int layer) : base(data, target, caster, layer)
    {
        paramMul = data.arrParam[0];
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
