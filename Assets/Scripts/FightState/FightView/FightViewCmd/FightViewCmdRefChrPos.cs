using DefaultNamespace;
using DG.Tweening;
using System;
using System.Collections.Generic;

/// <summary>
/// 刷新角色位置
/// </summary>
public class FightViewCmdRefChrPos : FightViewCmdBase
{
    private readonly Character character;
    private readonly bool withAnim;

    public FightViewCmdRefChrPos(Character character, bool withAnim) {
        this.character = character;
        this.withAnim = withAnim;
    }

    public override void Play()
    {
        base.Play();
        if (withAnim)
        {
            var toPosTarget = FightState.Inst.GetPosByTeamLoc(character.camp, character.teamLoc);
            character.entityCtl.transform.DOMove(toPosTarget, 0.5f);
        }
        else
        {
            character.entityCtl.SetPos(FightState.Inst.GetPosByTeamLoc(character.camp, character.teamLoc));
        }
        End();
    }
}
