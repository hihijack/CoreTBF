using UnityEngine;

/// <summary>
/// 伤害目标,并给目标添加一个buff
/// </summary>
public class FightActionAtk : FightActionBase
{
    public FightActionAtk(Skill skill, ActionContent actionContent) : base(skill, actionContent)
    {

    }

    /// <summary>
    /// 宣言阶段处理
    /// </summary>
    public override void PreAct()
    {
        base.PreAct();
        this.actionContent.caster.OnStartAttack(this.actionContent);
        var lstTargets = this.actionContent.targets;
        foreach (var target in lstTargets)
        {
            target.OnStartAttacked(this.actionContent);
        }
    }

    public override void PostAct()
    {
        base.PostAct();
    }

    public override void Act()
    {
        base.Act();
    }

    public override void RealAct()
    {
        base.RealAct();
        bool isPowerAct = IsPowerAct();
        var caster = actionContent.caster;
        var skillBaseData = skill.GetBaseData();

        if (isPowerAct)
        {
            //蓄力技能
            //蓄力开始
            caster.State = ECharacterState.Power;
            caster.mTimeStiff = skillBaseData.timePower;
            caster.mSkillPowering = skill;
            //蓄力改变韧性
            if (skillBaseData.tenChangeToPower > 0)
            {
                var oriVal = caster.propData.tenacity;
                caster.propData.SetTenacityPercent(skillBaseData.tenChangeToPower);
                var curVal = caster.propData.tenacity;
                FightState.Inst.eventRecorder.CacheEvent(new FightEventTenHurted(caster,oriVal, curVal - oriVal));
            }
        }
        else
        {
            caster.mSkillPowering = null;
            caster.mTimePower = 0;
            caster.State = ECharacterState.Acting;

            //韧性改变至指定百分比;0不改变
            if (skillBaseData.tenChangeTo > 0)
            {
                var oriVal = caster.propData.tenacity;
                caster.propData.SetTenacityPercent(skillBaseData.tenChangeTo);
                var curVal = caster.propData.tenacity;
                FightState.Inst.eventRecorder.CacheEvent(new FightEventTenHurted(caster, oriVal, curVal - oriVal));
            }

            ProcActEffect();

            //后摇硬直
            if (caster.IsEnableAction)
            {
                caster.State = ECharacterState.Stiff;
                caster.mTimeStiff = skillBaseData.backswing;
            }
        }
    }
}