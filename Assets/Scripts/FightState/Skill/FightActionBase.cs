using System;
using System.Collections.Generic;
using Data;
using DefaultNamespace;
using SimpleJSON;
using UI;
using UnityEngine;
using UnityEngine.Timeline;

public class FightActionBase
{
    public Character caster;
    public SkillBaseData skill;
    public List<Character> targets;

    public Action actionActEnd;

    public List<FightSkillProcessorBase> lstProcessor;

    public FightActionBase(Character caster, SkillBaseData skill, List<Character> targets)
    {
        this.caster = caster;
        this.skill = skill;
        this.targets = targets;
        ParseProcessor();
    }

    public virtual void Act()
    {
        UIFightLog.Inst.AppendLog($"{caster.roleData.name}发动了{skill.name}");


        TimelineAsset tlAssetToPlay;

        if (IsPowerAct())
        {
            //蓄力表现
            tlAssetToPlay = Resources.Load<TimelineAsset>($"TimeLines/{skill.tlAssetPower}");
        }
        else
        {
            tlAssetToPlay = Resources.Load<TimelineAsset>($"TimeLines/{skill.tlAsset}");
        }

        if (tlAssetToPlay != null)
        {
            //UI关闭AI提示显示
            UIFight.Inst.SetAIItemsVisible(false);
            FightState.Inst.fightViewBehav.Play(tlAssetToPlay);
        }
        else
        {
            RealAct();
            EndAct();
        }
    }

    /// <summary>
    /// 是否是蓄力开始行动
    /// </summary>
    /// <returns></returns>
    public bool IsPowerAct()
    {
        return skill.timePower > 0 && caster.mTimePower < skill.timePower;
    }

    public virtual void RealAct()
    {

    }

    public virtual void EndAct() { actionActEnd(); }

    /// <summary>
    /// 处理技能效果
    /// </summary>
    protected void ProcActEffect()
    {
        if (lstProcessor == null)
        {
            return;
        }
        foreach (var skillProc in lstProcessor)
        {
            skillProc.Proc();
        }
    }

    /// <summary>
    /// 技能效果处理器构建
    /// </summary>
    protected void ParseProcessor()
    {
        if (skill.data == null)
        {
            return;
        }
        lstProcessor = new List<FightSkillProcessorBase>();
        for (int i = 0; i < skill.data.Count; i++)
        {
            JSONNode node = skill.data[i];
            FightSkillConditionBase condition = null;
            if (node[FightSkillProcKey.CONDITION] == null)
            {
                condition = new FightSkillConditionNone(this, node);
            }
            switch (node[FightSkillProcKey.EFFECT])
            {
                case FightSkillProcVal.EFFECT_DEF:
                    lstProcessor.Add(new FightSkillProcDef(this, node, condition));
                    break;
                case FightSkillProcVal.EFFECT_DMG_TARGET:
                    lstProcessor.Add(new FightSkillProcDmgTarget(this, node, condition));
                    break;
                case FightSkillProcVal.ADD_BUFF:
                    lstProcessor.Add(new FightSkillProcAddBuff(this, node, condition));
                    break;
                case FightSkillProcVal.SUMMON:
                    lstProcessor.Add(new FightSkillProcSummon(this, node, condition));
                    break;
                default:
                    break;
            }
        }
    }
}