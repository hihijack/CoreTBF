using UnityEngine;
using System.Collections;
using DefaultNamespace;
using DG.Tweening;
using UI;
using System;

public class FightActionExchangeLoc : FightActionBase
{
    public override void Act()
    {

        UIMgr.Inst.uiFightLog.AppendLog($"{caster.roleData.name}发动了{skill.name}");

        var target = targets[0];
        var t = caster.teamLoc;
        caster.teamLoc = target.teamLoc;
        target.teamLoc = t;
        var toPosCaster = FightState.Inst.GetPosByTeamLoc(caster.camp, caster.teamLoc);
        var toPosTarget = FightState.Inst.GetPosByTeamLoc(target.camp, target.teamLoc);
        caster.entityCtl.transform.DOMove(toPosCaster, 0.5f).onComplete += OnAnimEnd;
        target.entityCtl.transform.DOMove(toPosTarget, 0.5f);
        //后摇硬直
        caster.State = ECharacterState.Stiff;
        caster.mTimeStiff = skill.backswing;
    }

    private void OnAnimEnd()
    {
        EndAct();
    }
}
