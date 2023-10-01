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
    public Skill skill;
    public ActionContent actionContent;//选择的目标。对自身释放的为空

    public Action actionActEnd;

    public FightActionBase(Skill skill, ActionContent actionContent)
    {
        this.skill = skill;
        this.actionContent = actionContent;
    }

    public Character Caster
    {
        get
        {
            return actionContent.caster;
        }
    }

    public List<Character> Targets
    {
        get
        {
            return actionContent.targets;
        }
    }

    public virtual void Act()
    {

        Debug.Log("释放技能:" + Caster.roleData.name + ",skill:" + skill.GetBaseData().name);//##########

        //预处理阶段
        PreAct();

        if (IsPowerAct())
        {
            FightState.Inst.eventRecorder.CacheEvent(new FightEventStartPower(Caster, skill));
        }
        else
        {
            FightState.Inst.eventRecorder.CacheEvent(new FightEventCastSkill(Caster, skill, Targets));
        }
        //效果发生
        RealAct();

        FightState.Inst.fightViewBehav.StartPlayCachedViewCmd(OnViewPlayEnd);
    }

    private void OnViewPlayEnd()
    {
        EndAct();

        //队伍全灭
        ECamp camDieOut;
        if (FightState.Inst.CheckATeamDieOut(out camDieOut))
        {
            FightState.Inst.OnTeamDieOut(camDieOut);
        }
    }

    /// <summary>
    /// 是否是蓄力开始行动
    /// </summary>
    /// <returns></returns>
    public bool IsPowerAct()
    {
        var skillBaseData = skill.GetBaseData();
        return skillBaseData.timePower > 0 && Caster.mTimePower < skillBaseData.timePower;
    }

    public virtual void RealAct()
    {

    }

    public virtual void EndAct() { actionActEnd(); }

    /// <summary>
    /// 宣言阶段处理
    /// </summary>
    public virtual void PreAct()
    {

    }

    /// <summary>
    /// 后处理阶段处理
    /// </summary>
    public virtual void PostAct()
    {

    }

    /// <summary>
    /// 处理技能效果
    /// </summary>
    protected void ProcActEffect()
    {
        //skill.Proc(actionContent);
        FightState.Inst.skillProcHandler.ActiveProc(actionContent);
    }
}