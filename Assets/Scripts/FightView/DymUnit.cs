using UnityEngine;
using System.Collections;
using System;
using DefaultNamespace;

public enum EDymUnitType
{
    Caster,
    TargetWithIndex
}

[Serializable]
public class DymUnit
{
    public EDymUnitType dymUnitType;
    public DymUnitCaster dymUnitCaster;
    public DymUnitTarget dymUnitTarget;

    public Character Get()
    {
        switch (dymUnitType)
        {
            case EDymUnitType.Caster:
                return dymUnitCaster.Get();
            case EDymUnitType.TargetWithIndex:
                return dymUnitTarget.Get();
            default:
                return null;
        }
    }
}
