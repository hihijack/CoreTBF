using Data;
using System.Collections.Generic;
using UI;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
    /// <summary>
    /// 伤害目标,并给目标添加一个buff
    /// </summary>
    public class FightActionAtk : FightActionBase
    {
        public FightActionAtk(Character caster, SkillBaseData skill, List<Character> targets) : base(caster, skill, targets)
        {

        }

        public override void Act()
        {
            base.Act();
        }

        public override void RealAct()
        {
            base.RealAct();
            bool isPowerAct = IsPowerAct();

            if (isPowerAct)
            {
                //蓄力技能
                //蓄力开始
                caster.State = ECharacterState.Power;
                caster.mTimeStiff = skill.timePower;
                caster.mSkillPowering = skill;
                //蓄力改变韧性
                if (skill.tenChangeToPower > 0)
                {
                    caster.propData.SetTenacityPercent(skill.tenChangeToPower);
                    UIHPRoot.Inst.RefreshTarget(caster);
                }
               
                UIFightLog.Inst.AppendLog($"{caster.roleData.name}开始蓄力:{skill.name}!!");
            }
            else
            {
                caster.mSkillPowering = null;
                caster.mTimePower = 0;
                caster.State = ECharacterState.Acting;

                //mp恢复
                if (caster.camp == ECamp.Ally)
                {
                    PlayerRolePropDataMgr.Inst.ChangeMP(skill.mpGet);
                }

                //韧性改变至指定百分比;0不改变
                if (skill.tenChangeTo > 0)
                {
                    caster.propData.SetTenacityPercent(skill.tenChangeTo);
                }

                UIHPRoot.Inst.RefreshTarget(caster);

                //技能效果
                //foreach (var target in targets)
                //{
                //    caster.DamageTarget(target, new DmgData() { dmgPrecent = skill.dmg, timeAtkStiff = skill.timeAtkStiff, tenAtk = skill.dmgTenacity });
                //    if (skill.data != null && skill.data["buff"] != null)
                //    {
                //        //添加buff
                //        target.AddABuff(skill.data["buff"].AsInt, skill.data["dur"].AsFloat, caster);
                //    }
                //}

                ProcActEffect();

                //后摇硬直
                caster.State = ECharacterState.Stiff;
                caster.mTimeStiff = skill.backswing;
            }
        }
    }
}