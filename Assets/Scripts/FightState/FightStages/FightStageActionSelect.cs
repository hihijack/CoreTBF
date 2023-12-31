﻿using System.Collections.Generic;
using UI;

namespace DefaultNamespace.FightStages
{
    /// <summary>
    /// 触发方行动选择
    /// </summary>
    public class FightStageActionSelect : FightStageBase
    {
        private Queue<Character> _queue = new Queue<Character>();
        
        public override void OnEnter()
        {
            base.OnEnter();

            //UIMgr.Inst.uiFightLog.AppendLog(">>>进入主要阶段");

            //在Ready状态的友方可选择行动
            //如果是防御状态,只能切换到等待
            //等待状态可以切换到其他行动
            //蓄力完毕进入,不可选择行动,直接使用蓄力技能
            
            var acter = FightState.Inst.GetActiveCharacter();
            
            if (acter.camp == ECamp.Ally)
            {
                //if (acter.mSkillPowering != null)
                //{
                //    //有正在蓄力的技能
                //    acter.ActionSelectPoweringSkill();
                //}
                
                UIMgr.Inst.ShowUI(UITable.EUITable.UIFightActionPanel);
                UIFightActionRoot.Inst.SetActionVisible(true);
                //UIMgr.Inst.uiFightActionRoot.StartShow();
            }
            else
            {
                //AI选择技能
                //if (acter.mSkillPowering != null)
                //{
                //    acter.ActionSelectPoweringSkill();
                //}
                //else
                //{

                //}

                FightState.Inst.characterMgr.CacheAllAIAction();

                FightState.Inst.ToNextStage();
            }

        }

        public override void OnExit()
        {
            base.OnExit();
            UIMgr.Inst.HideUI(UITable.EUITable.UIFightActionPanel);
            UIFight.Inst.RefreshAIItems();
        }
    }
}