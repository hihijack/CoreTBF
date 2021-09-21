using SimpleJSON;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FightSkillConditionFactory
{
    public static FightSkillConditionBase Create(JSONNode conditonNode)
    {
        FightSkillConditionBase condition = null;
        if (conditonNode == null)
        {
            condition = new FightSkillConditionNone(null);
        }
        else
        {
            var conditonType = conditonNode["type"].Value;
            if (conditonType.Equals(FightSkillConditionVal.HP_LINE))
            {
                condition = new FightSkillConditionHPLine(conditonNode);
            }
            else if (conditonType.Equals(FightSkillConditionVal.BUFF_LAYER))
            {
                condition = new FightSkillConditionBuffLayer(conditonNode);
            }
            else
            {
                Debug.LogError("错误的条件类型:" + conditonType);
            }
        }
        return condition;
    }
}
