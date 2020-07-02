using System.Collections.Generic;
using UI;

namespace DefaultNamespace.FightStages
{
    /// <summary>
    /// 行动结束阶段.在准备状态的可以再选择行动.
    /// </summary>
    public class FightStageActionEnd : FightStageBase
    {
        private Queue<Character> _queue = new Queue<Character>();
        
        public override void OnEnter()
        {
            base.OnEnter();
            if (GameMgr.Inst.IsSkipEndStage)
            {
                GameMgr.Inst.ToNextStage();
                return;
            }

           // UIMgr.Inst.uiFightLog.AppendLog(">>>进入结束阶段");

            UIMgr.Inst.uiFightActionRoot.SetVisible(true);
            UIMgr.Inst.uiFightActionRoot.SetActionVisible(true);
            //UIMgr.Inst.uiFightActionRoot.StartShow();
        }
    }
}