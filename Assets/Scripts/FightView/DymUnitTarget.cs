using UnityEngine;
using System.Collections;
using DefaultNamespace;
using System;

[Serializable]
public class DymUnitTarget : DymUnitBase
{
    public int index;

    public override Character Get()
    {
        Debug.Log("DymUnitTarget:" + index);
        return null;
    }
}
