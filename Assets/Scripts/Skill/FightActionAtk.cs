using UI;

namespace DefaultNamespace
{
    public class FightActionAtk : FightActionBase
    {
        public override void Act()
        {
            base.Act();

            bool realyAct = true;
            
            if (skill.timePower > 0)
            {
                //蓄力技能
                if (caster.mTimePower < skill.timePower)
                {
                    //蓄力开始
                    realyAct = false;
                    caster.State = ECharacterState.Power;
                    caster.mTimeStiff = skill.timePower;
                    caster.mSkillPowering = skill;
                    UIMgr.Inst.uiFightLog.AppendLog($"{caster.roleData.name}开始蓄力:{skill.name}!!");
                }
            }

            if (realyAct)
            {
                caster.mSkillPowering = null;
                caster.mTimePower = 0;
                caster.State = ECharacterState.Acting;
                foreach (var target in targets)
                {
                    caster.DamageTarget(target, skill.dmg, skill.timeAtkStiff);
                }
               
                //后摇硬直
                caster.State = ECharacterState.Stiff;
                caster.mTimeStiff = skill.backswing;
            }

            //行动结束
            actionEnd();
        }
    }
}