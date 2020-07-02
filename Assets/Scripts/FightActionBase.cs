using System;
using System.Collections.Generic;
using Data;
using UI;
using UnityEngine.Timeline;

namespace DefaultNamespace
{
    public class FightActionBase
    {
        public Character caster;
        public SkillData skill;
        public List<Character> targets;

        public Action actionActEnd;
        
        public virtual void Act()
        {
            UIMgr.Inst.uiFightLog.AppendLog($"{caster.roleData.name}发动了{skill.name}");


            TimelineAsset tlAssetToPlay; 

            if (IsPowerAct())
            {
                //蓄力表现
                tlAssetToPlay = skill.tlAssetPower;
            }
            else
            {
                tlAssetToPlay = skill.tlAsset;
            }

            if (tlAssetToPlay != null)
            {
                //UI关闭AI提示显示
                UIMgr.Inst.uiFight.SetAIItemsVisible(false);
                GameMgr.Inst.fightViewBehav.TimeLineCtl.Play(tlAssetToPlay);
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
    }
}