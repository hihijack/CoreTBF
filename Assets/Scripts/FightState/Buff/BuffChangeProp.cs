using UnityEngine;
using System.Collections;
using DefaultNamespace;
using System;
using UnityEngine.Experimental.TerrainAPI;

public enum EDefType
{
    /// <summary>
    /// 物理防御
    /// </summary>
    DefPhy = 0
}

public static class PropType
{
    public const string DEF = "def";
    public const string ATK = "atk";
    public const string RES_FIRE = "res_fire";
    public const string RES_THUNDER = "res_thunder";
    public const string RES_DARK = "res_dark";
    public const string RES_MAGIC = "res_magic";
    public const string TOUGHNESS = "toughness";
    public const string HURTEDMUL = "hurt";
    public const string SPEED = "speed";
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
            case PropType.DEF:
                //指定防御类型参数A增加add值,参数B增加mul值
                target.propData.defParamAdd += paramAdd;
                target.propData.defParamMul += paramMul;
                break;
            case PropType.ATK:
                target.propData.atkParamAdd += paramAdd;
                target.propData.atkParmaMul += paramMul;
                break;
            case PropType.TOUGHNESS:
                target.propData.toughnessParamAdd += paramAdd;
                target.propData.toughnessParamMul += paramMul;
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
            case PropType.DEF:
                //指定防御类型参数A增加add值,参数B增加mul值
                target.propData.defParamAdd -= paramAdd;
                target.propData.defParamMul -= paramMul;
                break;
            case PropType.ATK:
                target.propData.atkParamAdd -= paramAdd;
                target.propData.atkParmaMul -= paramMul;
                break;
            case PropType.TOUGHNESS:
                target.propData.toughnessParamAdd -= paramAdd;
                target.propData.toughnessParamMul -= paramMul;
                break;
            default:
                break;
        }
    }
}
