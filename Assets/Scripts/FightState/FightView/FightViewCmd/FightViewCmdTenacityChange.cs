using DefaultNamespace;
using UnityEngine;

public class FightViewCmdTenacityChange : FightViewCmdBase
{
    private readonly Character target;
    private readonly int oriVal;
    private readonly int curVal;

    public FightViewCmdTenacityChange(Character target, int oriVal, int curVal)
    {
        this.target = target;
        this.oriVal = oriVal;
        this.curVal = curVal;
    }

    public override void Play()
    {
        UIHPRoot.Inst.RefreshTargetTenWithVal(target, curVal);
        End();
    }
}
