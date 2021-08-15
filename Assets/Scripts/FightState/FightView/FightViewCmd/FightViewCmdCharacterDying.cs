using DefaultNamespace;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色濒死
/// </summary>
public class FightViewCmdCharacterDying : FightViewCmdBase
{
    private Character character;

    public FightViewCmdCharacterDying(Character character)
    {
        this.character = character;
    }

    public override void Play()
    {
        base.Play();
        Debug.Log("dying" + character.roleData.name);//#########
        character.entityCtl.SetSprite("dying");
        End();
    }
}
