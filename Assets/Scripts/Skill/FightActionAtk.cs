using UI;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
    public class FightActionAtk : FightActionBase
    {
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
                    UIMgr.Inst.uiHPRoot.RefreshTarget(caster);
                }
               
                UIMgr.Inst.uiFightLog.AppendLog($"{caster.roleData.name}开始蓄力:{skill.name}!!");
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

                UIMgr.Inst.uiHPRoot.RefreshTarget(caster);

                foreach (var target in targets)
                {
                    caster.DamageTarget(target, new DmgData() { dmgPrecent = skill.dmg, timeAtkStiff = skill.timeAtkStiff, tenAtk = skill.tenacityAtk });
                    //添加buff
                    foreach (var buffID in skill.buffsAdd)
                    {
                        target.AddABuff(buffID, caster);
                    }
                }

                //后摇硬直
                caster.State = ECharacterState.Stiff;
                caster.mTimeStiff = skill.backswing;
            }
        }
    }
}