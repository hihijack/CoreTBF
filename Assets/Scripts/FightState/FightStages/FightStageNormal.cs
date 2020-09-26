using UnityEngine;

namespace DefaultNamespace.FightStages
{
    public class FightStageNormal : FightStageBase
    {
        public override void OnUpdate()
        {
            base.OnUpdate();

            FightState.Inst.characterMgr.UpdateAllCharacterInNormalStage();
          
            //检测是否有进入触发
            if (FightState.Inst.GetActiveCharacter() != null)
            {
                FightState.Inst.SetFightStage(EFightStage.ActionSelect);
            }
            else
            {
                //等待中的开始行动
                //if (Input.GetKeyDown(KeyCode.Space))
                //{
                //    GameMgr.Inst.BtnActionAtOnce();
                //}
            }
        }
    }
}