using UnityEngine;
using System.Collections;
using DefaultNamespace;

public struct ParamEffectCreate
{
    public string effName;
    public Vector3 offsetPos;
    public bool bind;
}

public class EffectMgr
{
    public static GameObject CreateEffForUnit(RoleEntityCtl entity, ParamEffectCreate param)
    {
        var pfbEff = Resources.Load<GameObject>("Prefabs/Effects/" + param.effName);
        var goEff = GameUtil.PopOrInst(pfbEff);
        var pos = entity.transform.localToWorldMatrix.MultiplyPoint(param.offsetPos);
        goEff.transform.position = pos;
        if (param.bind)
        {
            goEff.transform.parent = entity.transform;
        }
        return goEff;
    }
}
