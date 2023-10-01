
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

    List<FightSkillActiveableProcBase> lstProcActived;

    public BuffBase(BuffBaseData data, Character target, Character caster, int layer, float dur)
    {
        this.baseData = data;
        this.target = target;
        this.caster = caster;
        this.layer = layer;
        this.dur = dur;
        valid = true;

        lstProcActived = new List<FightSkillActiveableProcBase>();

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

    internal void PassiveProc(ActionContent content)
    {
        foreach (var procer in lstProcessor)
        {
            if (procer.IsTried(content.tri) && procer.CheckConditon())
            {
                procer.Proc(content);
                if (content.tri == FightSkillTriType.ACTIVE)
                {
                    lstProcActived.Add(procer as FightSkillActiveableProcBase);
                }
            }
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

    public int GetLayer()
    {
        return layer;
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
                    switch (baseData.timeOutMode)
                    {
                        case EBuffTimeOutMode.Clear:
                            //设为0层,移除
                            SetLayer(0);
                            break;
                        case EBuffTimeOutMode.LayerReduce:
                            //层数-1
                            ChangeLayer(-1);
                            //重新计时
                            RestartDur();
                            break;
                        case EBuffTimeOutMode.LayerAdd:
                            //层数+1
                            ChangeLayer(1);
                            //重新计时
                            RestartDur();
                            break;
                        default:
                            break;
                    }
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
    
    public virtual void OnAdd() 
    {
        OnActive();
    }
    protected virtual void OnChangeLayer() 
    {
        PassiveTried(FightSkillTriType.LAYER_CHANGE, null);
    }
    protected virtual void OnRemoved() 
    {
        OnUnActive();
    }


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

            var conditonNode = node[FightSkillProcKey.CONDITION];
            condition = FightSkillConditionFactory.Create(conditonNode);

            FightSkillProcessorBase processor = FightSkillProcessorFactory.Crate(this, node, condition);
            if (processor != null)
            {
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
    /// 生效时
    /// </summary>
    public void OnActive()
    {
        PassiveTried(FightSkillTriType.ACTIVE, null);
    }


    /// <summary>
    /// 失效时
    /// </summary>
    public void OnUnActive()
    {
        foreach (var proc in lstProcActived)
        {
            proc.UnProc(null);
        }
    }

    /// <summary>
    /// 被动触发
    /// </summary>
    /// <param name="content"></param>
    public void PassiveTried(string tri, ActionContent content)
    {
        bool tried = false;
        foreach (var proc in lstProcessor)
        {
            if (proc.IsTried(tri) && proc.CheckConditon())
            {
                tried = true;
                break;
            }
        }
        if (tried)
        {
            var newContent = ActionContentFactory.Create(GetOwnerCharacter(), this, content, tri);
            FightState.Inst.skillProcHandler.EnqueueNode(newContent);
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

    public void OnBeforeEndTurn(ActionContent sourceContent)
    {
        PassiveTried(FightSkillTriType.BEFORE_END_TRUN, sourceContent);
    }
}
