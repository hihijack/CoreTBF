using UnityEngine;
using System.Collections;
using DefaultNamespace;
using System;

public enum EDefType
{
    /// <summary>
    /// 物理防御
    /// </summary>
    DefPhy = 0
}

public class BuffChangeDef : BuffBase
{

    EDefType defType;
    int paramAdd;
    float paramMul;

    public BuffChangeDef(BuffBaseData data, Character target, Character caster, int layer, float dur) : base(data, target, caster, layer, dur)
    {
        defType = (EDefType)Enum.Parse(typeof(EDefType), data.data["type"].ToString()) ;
        paramAdd = data.data["valAdd"].AsInt;
        paramMul = data.data["valMul"].AsFloat;
    }

    public override void OnAdd()
    {
        base.OnAdd();
        //指定防御类型参数A增加add值,参数B增加mul值
        target.propData.defParamAdd += paramAdd;
        target.propData.defParamMul += paramMul;
    }

    protected override void OnRemoved()
    {
        base.OnRemoved();
        target.propData.defParamAdd -= paramAdd;
        target.propData.defParamMul -= paramMul;
    }
}
