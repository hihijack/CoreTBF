using UnityEngine;
using System.Collections;
using DefaultNamespace;

public class BuffFactory 
{
    public static BuffBase CreateABuff(int buffID, Character target, Character caster)
    {
        var buffData = GameData.Inst.buffData.Get(buffID);
        switch (buffData.logic)
        {
            case EBuffLogic.ChangeDef:
                return new BuffChangeDef(buffData, target, caster, 1);
            case EBuffLogic.Break:
                return new BuffBreak(buffData, target, caster, 1);
            default:
                break;
        }
        return null;
    }
}
