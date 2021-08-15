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
        //var caster = actionContent.caster;
        //var skillBaseData = skill.GetBaseData();

        //UIFightLog.Inst.AppendLog($"{caster.roleData.name}发动了{skillBaseData.name}");


        //TimelineAsset tlAssetToPlay;

        //if (IsPowerAct())
        //{
        //    //蓄力表现
        //    tlAssetToPlay = Resources.Load<TimelineAsset>($"TimeLines/{skillBaseData.tlAssetPower}");
        //}
        //else
        //{
        //    tlAssetToPlay = Resources.Load<TimelineAsset>($"TimeLines/{skillBaseData.tlAsset}");
        //}

        //if (tlAssetToPlay != null)
        //{
        //    //UI关闭AI提示显示
        //    UIFight.Inst.SetAIItemsVisible(false);
        //    FightState.Inst.fightViewBehav.Play(tlAssetToPlay);
        //}
        //else
        //{
        //    RealAct();
        //    EndAct();
        //    FightState.Inst.characterMgr.HandleHPState();
        //    ECamp camDieOut;
        //    if (FightState.Inst.CheckATeamDieOut(out camDieOut))
        //    {
        //        FightState.Inst.OnTeamDieOut(camDieOut);
        //    }
        //}

        FightState.Inst.fightViewBehav.CacheViewCmd(
            new FightViewCmdPreCastSkill(new FightViewCmdPreCastSkillData() { target = this.Caster, skill = this.skill}));
        //预处理阶段
        PreAct();

        FightState.Inst.fightViewBehav.CacheViewCmd(
            new FightViewCmdCastSkill(new FightViewCmdCastSkillData() { caster = this.Caster, targets = this.Targets, skill = this.skill}));
        //效果发生
        RealAct();

        //生命状态处理
        //FightState.Inst.characterMgr.HandleHPState(actionContent);

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
        var caster = actionContent.caster;
        return skillBaseData.timePower > 0 && caster.mTimePower < skillBaseData.timePower;
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
        skill.Proc(actionContent);
    }
}