using System;
using System.Collections.Generic;
using Data;
using UI;
using UnityEngine;
using UnityEngine.Timeline;

namespace DefaultNamespace
{
    public class FightActionBase
    {
        public Character caster;
        public SkillBaseData skill;
        public List<Character> targets;

        public Action actionActEnd;
        
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
                FightState.Inst.fightViewBehav.TimeLineCtl.Play(tlAssetToPlay);
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