using UnityEngine;
using System.Collections;
using DefaultNamespace;

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

    public BuffChangeDef(BuffData data, Character target, Character caster, int layer) : base(data, target, caster, layer)
    {
        int iDefType = (int)data.arrParam[0];
        defType = (EDefType)iDefType;
        paramAdd = (int)data.arrParam[1];
        paramMul = data.arrParam[2];
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
