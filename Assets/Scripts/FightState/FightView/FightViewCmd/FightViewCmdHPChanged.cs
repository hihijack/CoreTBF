using DefaultNamespace;
using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

public enum EHPChangedType
{
    Hurted,
    Healed,
    ToDying,
    Killed //直接杀死
}

/// <summary>
/// 血量改变
/// </summary>
public class FightViewCmdHPChanged : FightViewCmdBase
{
    private readonly Character target;
    private readonly int oriVal;
    private readonly int curVal;
    private EHPChangedType changeType;

    public FightViewCmdHPChanged(Character target, int oriVal, int curVal, EHPChangedType type)
    {
        this.target = target;
        this.oriVal = oriVal;
        this.curVal = curVal;
        this.changeType = type;
    }


    public override void Play()
    {
        UIHPRoot.Inst.RefreshTargetHPWithVal(target, curVal);
        End();
    }
}
