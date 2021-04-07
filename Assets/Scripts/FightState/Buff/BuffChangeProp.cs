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

public static class PropType
{
    public const string DEF_PHY = "def_phy";
    public const string ATK = "atk";
}


public class BuffChangeProp : BuffBase
{

    string propType;
    int paramAdd;
    float paramMul;

    public BuffChangeProp(BuffBaseData data, Character target, Character caster, int layer, float dur) : base(data, target, caster, layer, dur)
    {
        propType = data.data["prop"];
        paramAdd = data.data["valAdd"].AsInt;
        paramMul = data.data["valMul"].AsFloat;
    }

    public override void OnAdd()
    {
        base.OnAdd();
        switch (propType)
        {
            case PropType.DEF_PHY:
                //指定防御类型参数A增加add值,参数B增加mul值
                target.propData.defParamAdd += paramAdd;
                target.propData.defParamMul += paramMul;
                break;
            case PropType.ATK:
                target.propData.atkParamAdd += paramAdd;
                target.propData.atkParmaMul += paramMul;
                break;
            default:
                break;
        }
    }

    protected override void OnRemoved()
    {
        base.OnRemoved();
        switch (propType)
        {
            case PropType.DEF_PHY:
                //指定防御类型参数A增加add值,参数B增加mul值
                target.propData.defParamAdd -= paramAdd;
                target.propData.defParamMul -= paramMul;
                break;
            case PropType.ATK:
                target.propData.atkParamAdd -= paramAdd;
                target.propData.atkParmaMul -= paramMul;
                break;
            default:
                break;
        }
    }
}
