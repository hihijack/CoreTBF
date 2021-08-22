using DefaultNamespace;
using UnityEngine;

public class FightViewCmdMPChange : FightViewCmdBase
{
    private readonly int oriVal;
    private readonly int curVal;

    public FightViewCmdMPChange(int oriVal, int curVal)
    {
        this.oriVal = oriVal;
        this.curVal = curVal;
    }

    public override void Play()
    {
        Debug.Log($"MPRef:{curVal}");//##########
        UIHPRoot.Inst.RefreshMPWithVal(curVal);
        End();
    }
}
