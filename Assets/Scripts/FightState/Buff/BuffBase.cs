
using DefaultNamespace;
using SimpleJSON;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BuffBase : ITriggedable,ISkillProcOwner
{
    public float curDur;

    protected bool valid;
    protected BuffBaseData baseData;
    protected Character target;
    protected Character caster;
    protected int layer;

    float dur;

    List<FightSkillProcessorBase> lstProcessor;

    public BuffBase(BuffBaseData data, Character target, Character caster, int layer, float dur)
    {
        this.baseData = data;
        this.target = target;
        this.caster = caster;
        this.layer = layer;
        this.dur = dur;
        valid = true;

        ParseProcessor();
    }

    public BuffBaseData GetBuffData()
    {
        return baseData;
    }

    public void ChangeLayer(int layerOffset)
    {
        layer += layerOffset;
        layer = Mathf.Clamp(layer, 0, baseData.maxLayer);
        OnChangeLayer();
        if (layer <= 0)
        {
            //移除
            valid = false;
            OnRemoved();
        }
    }

    public bool IsValid()
    {
        return valid;
    }

    public void SetLayer(int layer)
    {
        this.layer = layer;
        layer = Mathf.Clamp(layer, 0, baseData.maxLayer);
        OnChangeLayer();
        if (layer <= 0)
        {
            //移除
            valid = false;
            OnRemoved();
        }
    }

    public void UpdateDur(float time)
    {
        if (valid)
        {
            curDur += time;
            if (dur > 0)
            {
                if (curDur > dur)
                {
                    //设为0层,移除
                    SetLayer(0);
                }
            }
        }
    }

    public float GetDurLeft()
    {
        if (dur > 0)
        {
            return dur - curDur;
        }
        else
        {
            //永久
            return -1;
        }
    }

    public float GetDurProg()
    {
        if (dur > 0)
        {
            return curDur / dur;
        }
        else
        {
            //永久
            return -1;
        }
    }

    //刷新持续时间
    public void RestartDur() 
    {
        curDur = 0;
    }
    
    public virtual void OnAdd() { }
    protected virtual void OnChangeLayer() { }
    protected virtual void OnRemoved() { }


    /// <summary>
    /// 技能效果处理器构建
    /// </summary>
    protected void ParseProcessor()
    {
        lstProcessor = new List<FightSkillProcessorBase>();
        for (int i = 0; i < baseData.data.Count; i++)
        {
            JSONNode node = baseData.data[i];
            FightSkillConditionBase condition = null;
            if (node[FightSkillProcKey.CONDITION] == null)
            {
                condition = new FightSkillConditionNone(node);
            }

            FightSkillProcessorBase processor = null;

            switch (node[FightSkillProcKey.EFFECT])
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
                case FightSkillProcVal.SUMMON:
                    processor = (new FightSkillProcSummon(this, node, condition));
                    break;
                case FightSkillProcVal.GET_MP:
                    processor = new FightSkillProcGetMP(this, node, condition);
                    break;
                default:
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

    /// <summary>
    /// 被动触发
    /// </summary>
    /// <param name="content"></param>
    public void PassiveTried(string tri, ActionContent content)
    {
        foreach (var procer in lstProcessor)
        {
            if (procer.IsTried(tri))
            {
                procer.Proc(content);
            }
        }
    }

    internal List<FightSkillProcessorBase> GetProcs()
    {
        return lstProcessor;
    }

    public Character GetOwnerCharacter()
    {
        return target;
    }

    public Skill GetOwnerSkill()
    {
        return null;
    }
}
