using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FightSkillProcessorFactory
{
    public static FightSkillProcessorBase Crate(ISkillProcOwner owner, JSONNode node, FightSkillConditionBase condition)
    {
        FightSkillProcessorBase processor = null;
        var effectKey = node[FightSkillProcKey.EFFECT];
        switch (effectKey)
        {
            case FightSkillProcVal.EFFECT_DEF:
                processor = (new FightSkillProcDef(owner, node, condition));
                break;
            case FightSkillProcVal.EFFECT_DMG_TARGET:
                processor = (new FightSkillProcDmgTarget(owner, node, condition));
                break;
            case FightSkillProcVal.ADD_BUFF:
                processor = (new FightSkillProcAddBuff(owner, node, condition));
                break;
            case FightSkillProcVal.REMOVE_BUFF:
                processor = new FightSkillProcRemoveBuff(owner, node, condition);
                break;
            case FightSkillProcVal.SUMMON:
                processor = (new FightSkillProcSummon(owner, node, condition));
                break;
            case FightSkillProcVal.GET_MP:
                processor = new FightSkillProcGetMP(owner, node, condition);
                break;
            case FightSkillProcVal.HEAL_TARGET:
                processor = new FightSkillProcHealTarget(owner, node, condition);
                break;
            case FightSkillProcVal.CHANGE_PROP:
                processor = new FightSkillProcChangeProp(owner, node, condition);
                break;
            case FightSkillProcVal.CHANGE_AI:
                processor = new FightSkillProcChangeAI(owner, node, condition);
                break;
            case FightSkillProcVal.KILL:
                processor = new FightSkillProcKill(owner, node, condition);
                break;
            case FightSkillProcVal.HURTED:
                processor = new FightSkillProcHurted(owner, node, condition);
                break;
            default:
                Debug.LogError("无效的处理器:" + effectKey);//#######
                break;
        }

        if (processor != null)
        {
            //触发器
            var triNode = node[FightSkillProcKey.TIRS];
            if (triNode != null)
            {
                for (int triIndex = 0; triIndex < triNode.Count; triIndex++)
                {
                    processor.AddTri(triNode[triIndex]);
                }
            }
        }
        return processor;
    }
}
