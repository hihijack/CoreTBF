using DefaultNamespace;
using System;
using System.Collections.Generic;
using UI;

public struct FightViewCmdHpChangedData
{
    public Character target;
    public int oriVal;
    public int curVal;
}

/// <summary>
/// 血量改变
/// </summary>
public class FightViewCmdHPChanged : FightViewCmdBase
{
    private readonly Character target;
    private readonly int oriVal;
    private readonly int curVal;

    public FightViewCmdHPChanged(Character target, int oriVal, int curVal)
    {
        this.target = target;
        this.oriVal = oriVal;
        this.curVal = curVal;
    }

    public override void Play()
    {
        UIHPRoot.Inst.RefreshTargetHPWithVal(target, curVal);
        End();
    }
}
