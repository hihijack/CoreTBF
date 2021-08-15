using UnityEngine;
using System.Collections;
using DefaultNamespace;
using DG.Tweening;
using UI;
using System;
using Data;
using System.Collections.Generic;

public class FightActionExchangeLoc : FightActionBase
{
    public FightActionExchangeLoc(Skill skill, ActionContent content) : base(skill, content)
    {

    }

    public override void Act()
    {
        var caster = actionContent.caster;
        var target = actionContent.targets[0];
        var skillBaseData = skill.GetBaseData();
        UIFightLog.Inst.AppendLog($"{caster.roleData.name}发动了{skillBaseData.name}");

        var t = caster.teamLoc;
        caster.teamLoc = target.teamLoc;
        target.teamLoc = t;
        var toPosCaster = FightState.Inst.GetPosByTeamLoc(caster.camp, caster.teamLoc);
        var toPosTarget = FightState.Inst.GetPosByTeamLoc(target.camp, target.teamLoc);
        caster.entityCtl.transform.DOMove(toPosCaster, 0.5f).onComplete += OnAnimEnd;
        target.entityCtl.transform.DOMove(toPosTarget, 0.5f);
        //后摇硬直
        caster.State = ECharacterState.Stiff;
        caster.mTimeStiff = skillBaseData.backswing;
    }

    private void OnAnimEnd()
    {
        EndAct();
    }
}
