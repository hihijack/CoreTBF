using UnityEngine;

namespace DefaultNamespace.FightStages
{
    public class FightStageNormal : FightStageBase
    {
        public override void OnUpdate()
        {
            base.OnUpdate();

            FightState.Inst.characterMgr.UpdateAllCharacterInNormalStage();

            Debug.Log($"t[{Time.frameCount}]>>Update nomal stage");//##########

            //检测是否有表现命令
            if (FightState.Inst.fightViewBehav.HasCmdCached())
            {
                Debug.Log($"t[{Time.frameCount}]>>HasCmdCached");//##########
                //进入表现阶段
                FightState.Inst.SetFightStage(EFightStage.NormalView);
            }
            else
            {
                //检测是否有进入触发
                if (FightState.Inst.GetActiveCharacter() != null)
                {
                    FightState.Inst.SetFightStage(EFightStage.ActionSelect);
                }
            }
        }
    }
}