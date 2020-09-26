using UnityEngine;
using System.Collections;
using DefaultNamespace;

public class BuffFactory 
{
    public static BuffBase CreateABuff(int buffID, float dur, Character target, Character caster)
    {
        var buffData = BuffDataer.Inst.Get(buffID);
        switch (buffData.logic)
        {
            case EBuffLogic.ChangeDef:
                return new BuffChangeDef(buffData, target, caster, 1, dur);
            case EBuffLogic.Break:
                return new BuffBreak(buffData, target, caster, 1, dur);
            default:
                break;
        }
        return null;
    }
}
