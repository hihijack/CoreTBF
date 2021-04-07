using DefaultNamespace;
using SimpleJSON;
using System;
using System.Collections.Generic;


public static class FightSkillProcKey
{
    public const string EFFECT = "effect";
    public const string CONDITION = "condition";
}

public static class FightSkillProcVal
{
    public const string EFFECT_NONE = "none";
    public const string EFFECT_DEF = "def";
    public const string EFFECT_DMG_TARGET = "dmg_target";
    public const string ADD_BUFF = "add_buff";
    public const string SUMMON = "summon";
    public const string CONDITION_NONE = "none";
}

/// <summary>
/// 技能效果目标
/// </summary>
public static class SkillProcTarget
{
    public const string Targets = "targets"; //所有目标
    public const string Self = "self"; //自身
    public const string RanTarget = "rantarget"; //随机目标
}

/// <summary>
/// 技能效果处理器
/// </summary>
public abstract class FightSkillProcessorBase
{
    protected FightActionBase fightAction;
    protected FightSkillConditionBase condition;
    public FightSkillProcessorBase(FightActionBase fightAction, JSONNode jsonData, FightSkillConditionBase condition)
    {
        this.fightAction = fightAction;
        this.condition = condition;
        ParseFrom(jsonData);
    }

    protected abstract void ParseFrom(JSONNode jsonData);
    public abstract void Proc();
}
