using DefaultNamespace;
using System;
using System.Collections.Generic;

/// <summary>
/// 角色死亡
/// </summary>
public class FightViewCmdCharacterDie : FightViewCmdBase
{
    Character target;
    public FightViewCmdCharacterDie(Character target)
    {
        this.target = target;
    }

    public override void Play()
    {
        base.Play();
        target.PlayAnim("die");
    }

    public override void Update(float dt)
    {
        base.Update(dt);
        if (durTime >= 0.5f)
        {
            End();
        }
    }
}
