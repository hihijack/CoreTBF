using DefaultNamespace;
using SimpleJSON;
using System;
using System.Collections.Generic;

/// <summary>
/// 改变属性
/// </summary>
public class FightSkillProcChangeProp : FightSkillActiveableProcBase
{
    string m_prop;
    int m_valAdd;
    float m_valMul;

    public FightSkillProcChangeProp(ISkillProcOwner owner, JSONNode jsonData, FightSkillConditionBase condition) : base(owner, jsonData, condition)
    {
    }

    public override List<Character> GetTargets(ActionContent content)
    {
        return null;
    }

    public override SkillProcResult Proc(ActionContent content)
    {
        var target = owner.GetOwnerCharacter();
        switch (m_prop)
        {
            case PropType.DEF:
                //指定防御类型参数A增加add值,参数B增加mul值
                target.propData.defParamAdd += m_valAdd;
                target.propData.defParamMul *= m_valMul;
                break;
            case PropType.ATK:
                target.propData.atkParamAdd += m_valAdd;
                target.propData.atkParmaMul *= m_valMul;
                break;
            case PropType.TOUGHNESS:
                target.propData.toughnessParamAdd += m_valAdd;
                target.propData.toughnessParamMul *= m_valMul;
                break;
            case PropType.HURTEDMUL:
                target.propData.dmgHurtedMul *= m_valMul;
                break;
            default:
                break;
        }
        return new SkillProcResult();
    }

    public override SkillProcResult UnProc(ActionContent content)
    {
        var target = owner.GetOwnerCharacter();
        switch (m_prop)
        {
            case PropType.DEF:
                //指定防御类型参数A增加add值,参数B增加mul值
                target.propData.defParamAdd -= m_valAdd;
                target.propData.defParamMul /= m_valMul;
                break;
            case PropType.ATK:
                target.propData.atkParamAdd -= m_valAdd;
                target.propData.atkParmaMul /= m_valMul;
                break;
            case PropType.TOUGHNESS:
                target.propData.toughnessParamAdd -= m_valAdd;
                target.propData.toughnessParamMul /= m_valMul;
                break;
            case PropType.HURTEDMUL:
                target.propData.dmgHurtedMul /= m_valMul;
                break;
            default:
                break;
        }
        return new SkillProcResult();
    }

    protected override void ParseFrom(JSONNode jsonData)
    {
        m_prop = jsonData["prop"];
        var tValAdd = jsonData["val_add"];
        var tValMul = jsonData["val_mul"];

        m_valAdd = tValAdd != null ? tValAdd.AsInt : 0;
        m_valMul = tValMul != null ? tValMul.AsFloat : 0f;
    }
}
