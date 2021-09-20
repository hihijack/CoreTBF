using Data;
using DefaultNamespace;
using SimpleJSON;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using UnityEngine;

public class Skill : ITriggedable,ISkillProcOwner
{
    SkillBaseData baseData;
    List<FightSkillProcessorBase> lstProcessor;
    Character owner;
    public Skill(Character owner, SkillBaseData baseData)
    {
        this.owner = owner;
        this.baseData = baseData;
        ParseProcessor();
    }

    public SkillBaseData GetBaseData()
    {
        return baseData;
    }

    /// <summary>
    /// 技能效果处理器构建
    /// </summary>
    protected void ParseProcessor()
    {
        if (baseData == null || baseData.data == null)
        {
            return;
        }

        lstProcessor = new List<FightSkillProcessorBase>();
        for (int i = 0; i < baseData.data.Count; i++)
        {
            JSONNode node = baseData.data[i];
            
            FightSkillConditionBase condition = null;

            var conditonNode = node[FightSkillProcKey.CONDITION];
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
                else
                {
                    Debug.LogError("错误的条件类型:" + conditonType);
                }
            }

            FightSkillProcessorBase processor = null;

            var effectKey = node[FightSkillProcKey.EFFECT].Value;
            switch (effectKey)
            {
                case FightSkillProcVal.EFFECT_DEF:
                    processor = (new FightSkillProcDef(this, node, condition));
                    break;
                case FightSkillProcVal.EFFECT_DMG_TARGET:
                    processor = (new FightSkillProcDmgTarget(this, node, condition));
                    break;
                case FightSkillProcVal.ADD_BUFF:
                    processor = (new FightSkillProcAddBuff(this, node, condition));
                    break;
                case FightSkillProcVal.REMOVE_BUFF:
                    processor = new FightSkillProcRemoveBuff(this, node, condition);
                    break;
                case FightSkillProcVal.SUMMON:
                    processor = (new FightSkillProcSummon(this, node, condition));
                    break;
                case FightSkillProcVal.GET_MP:
                    processor = new FightSkillProcGetMP(this, node, condition);
                    break;
                case FightSkillProcVal.HEAL_TARGET:
                    processor = new FightSkillProcHealTarget(this, node, condition);
                    break;
                case FightSkillProcVal.CHANGE_LOC:
                    processor = new FightSkillProcChangeLoc(this, node, condition);
                    break;
                case FightSkillProcVal.CHANGE_AI:
                    processor = new FightSkillProcChangeAI(this, node, condition);
                    break;
                default:
                    Debug.LogError("无效的处理器:" + effectKey);
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

                lstProcessor.Add(processor);
            }

        }
    }

    public void OnDie(ActionContent sourceContent)
    {
        PassiveTried(FightSkillTriType.DIE, sourceContent);
    }

    public void OnHitted(ActionContent sourceContent)
    {
        PassiveTried(FightSkillTriType.HITED, sourceContent);
    }

    public void OnHurtd(ActionContent sourceContent)
    {
        PassiveTried(FightSkillTriType.HURTED, sourceContent);
    }

    public void OnStartAttack(ActionContent sourceContent)
    {
        PassiveTried(FightSkillTriType.START_ATTACK, sourceContent);
    }

    public void OnStartAttacked(ActionContent sourceContent)
    {
        PassiveTried(FightSkillTriType.START_ATTACKED, sourceContent);
    }

    public List<FightSkillProcessorBase> GetPros()
    {
        return lstProcessor;
    }

    /// <summary>
    /// 被动触发
    /// </summary>
    /// <param name="tri"></param>
    /// <param name="content"></param>
    private void PassiveTried(string tri, ActionContent content)
    {
        if (lstProcessor == null)
        {
            return;
        }

        ///触发次数限制
        if (!owner.procTimeMgr.CheckTimeLimit(this))
        {
            return;
        }

        bool tried = false;

        List<Character> targets = null;

        foreach (var proc in lstProcessor)
        {
            if (proc.IsTried(tri) && proc.CheckConditon())
            {
                tried = true;
                var targetsT = proc.GetTargets(content);
                proc.CacheTarget(targetsT);
                if (targetsT != null)
                {
                    targets = targetsT;
                }
            }
        }

        if (tried)
        {
            FightState.Inst.fightViewBehav.CacheViewCmd(new FightViewCmdCastSkill(this.owner, targets, this, false));
        }

        foreach (var proc in lstProcessor)
        {
            if (proc.IsTried(tri) && proc.CheckConditon())
            {
                Debug.Log("t>>" + owner.roleData.name + "被动触发:" + tri);//##########
                proc.Proc(content);
                proc.CacheTarget(null);
            }
        }

        if (tried)
        {
            owner.OnSkillProc(this);
        }
    }

    /// <summary>
    /// 主动使用
    /// </summary>
    /// <param name="content"></param>
    public void Proc(ActionContent content)
    {
        if (lstProcessor == null)
        {
            return;
        }

        bool success = false;

        foreach (var proc in lstProcessor)
        {
            if (proc.IsActive() && proc.CheckConditon())
            {
                proc.Proc(content);
                success = true;
            }
        }

        if (success)
        {
            //成功发动
            owner.OnSkillProc(this);
        }
    }

    public Character GetOwnerCharacter()
    {
        return owner;
    }

    public Skill GetOwnerSkill()
    {
        return this;
    }

    /// <summary>
    /// 次数限制
    /// </summary>
    /// <returns></returns>
    public bool IsTimeLimitEnable()
    {
        return GetOwnerCharacter().procTimeMgr.CheckTimeLimit(this);
    }

    public void OnBeforeEndTurn(ActionContent sourceContent)
    {
        PassiveTried(FightSkillTriType.BEFORE_END_TRUN, sourceContent);
    }
}
