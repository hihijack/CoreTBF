using DefaultNamespace;
using System;
using System.Collections.Generic;

public struct FightViewCmdPreCastSkillData
{
    public Character target;
    public Skill skill;
}

/// <summary>
/// 准备释放技能
/// </summary>
public class FightViewCmdPreCastSkill : FightViewCmdBase
{
    FightViewCmdPreCastSkillData data;

    public FightViewCmdPreCastSkill(FightViewCmdPreCastSkillData data)
    {
        this.data = data;
    }

    public override void Play()
    {
        base.Play();
        End();
    }
}
