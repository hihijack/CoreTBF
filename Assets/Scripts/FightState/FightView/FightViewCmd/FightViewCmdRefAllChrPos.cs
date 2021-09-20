using DefaultNamespace;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

struct ViewRefChrPosNode
{
    public Character chr;
    public Vector3 pos;
}

/// <summary>
/// 刷新指定阵营的所有角色位置
/// </summary>
public class FightViewCmdRefAllChrPos : FightViewCmdBase
{
    ECamp camp;
    bool withAnim;
    List<ViewRefChrPosNode> lstNode;

    Sequence tweenSeq;

    public FightViewCmdRefAllChrPos(ECamp camp, bool withAnim)
    {
        this.camp = camp;
        this.withAnim = withAnim;
        lstNode = new List<ViewRefChrPosNode>();
        var lstCharacters = FightState.Inst.characterMgr.GetCharactersOfCamp(camp);
        foreach (var chara in lstCharacters)
        {
            if (chara.IsAlive())
            {
                var posT = FightState.Inst.GetPosByTeamLoc(chara.camp, chara.teamLoc);
                lstNode.Add(new ViewRefChrPosNode() { chr = chara, pos = posT });
            }
        }
    }

    public override void Play()
    {
        base.Play();

        foreach (var node in lstNode)
        {
            if (withAnim)
            {
                if (tweenSeq == null)
                {
                    tweenSeq = DOTween.Sequence();
                    tweenSeq.onComplete += OnAnimEnd;
                }
                var doMove = node.chr.entityCtl.transform.DOMove(node.pos, 0.2f);
                tweenSeq.Join(doMove);
            }
            else
            {
                node.chr.entityCtl.transform.position = node.pos;
            }
        }

        if (!withAnim)
        {
            End();
        }
    }

    private void OnAnimEnd()
    {
        End();
    }
}
