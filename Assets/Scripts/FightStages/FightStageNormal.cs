using UnityEngine;

namespace DefaultNamespace.FightStages
{
    public class FightStageNormal : FightStageBase
    {
        public override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var character in GameMgr.Inst.lstCharacters)
            {
                character.UpdateInNormalStage();
            }
            //检测是否有进入触发
            if (GameMgr.Inst.GetActiveCharacter() != null)
            {
                GameMgr.Inst.SetFightStage(EFightStage.ActionSelect);
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