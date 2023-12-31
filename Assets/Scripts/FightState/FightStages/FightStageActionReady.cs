﻿using System.Collections.Generic;
using UI;

namespace DefaultNamespace.FightStages
{
    /// <summary>
    /// 触发方敌对方速攻选择
    /// </summary>
    public class FightStageActionReady : FightStageBase
    {        
        public override void OnEnter()
        {
            base.OnEnter();

            if (FightState.Inst.IsSkipReadyStage)
            {
                FightState.Inst.ToNextStage();
                return;
            }

            //UIMgr.Inst.uiFightLog.AppendLog(">>>进入速攻反击阶段");

            //如果触发方是敌人,我方准备状态中,有速攻技的可以发动速攻技能
            //如果触发方是我方,AI可以选择速攻技能
            var activer = FightState.Inst.GetActiveCharacter();
            if (activer.camp == ECamp.Enemy)
            {
                UIMgr.Inst.ShowUI(UITable.EUITable.UIFightActionPanel);
                UIFightActionRoot.Inst.SetActionVisible(true);
                //UIMgr.Inst.uiFightActionRoot.StartShow();
            }
            else
            {
                UIFightActionRoot.Inst.SetActionVisible(false);
                //TODO AI速攻技能
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }
        
        public override void OnExit()
        {
            base.OnExit();
            UIMgr.Inst.HideUI(UITable.EUITable.UIFightActionPanel);
        }
    }
}