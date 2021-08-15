using DefaultNamespace;
using SimpleJSON;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class FightSkillProcKey
{
    public const string EFFECT = "effect";
    public const string CONDITION = "condition";
    public const string TIRS = "tris";
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
/// 触发类型
/// </summary>
public static class FightSkillTriType
{
    public const string DIE = "die"; //死亡时
    public const string HITED = "hited"; //被击中时
    public const string HURTED = "hurted";//受伤时
    public const string HIT = "hit"; //命中目标
    public const string START_ATTACK = "start_attack"; //开始攻击
    public const string START_ATTACKED = "start_attacked"; //开始受击时
}

/// <summary>
/// 技能效果目标
/// </summary>
public static class SkillProcTarget
{
    public const string Targets = "target"; //所有目标
    public const string Self = "self"; //自身
    public const string RanTarget = "rantarget"; //随机目标
}

/// <summary>
/// 技能处理结果
/// </summary>
public struct SkillProcResult
{
    public List<Character> targets;
}

/// <summary>
/// 技能效果处理器
/// </summary>
public abstract class FightSkillProcessorBase
{
    protected FightSkillConditionBase condition;
    List<string> mLstTri; //触发器

    public ISkillProcOwner owner;

    protected List<Character> m_cacheTargets;

    public FightSkillProcessorBase(ISkillProcOwner owner, JSONNode jsonData, FightSkillConditionBase condition)
    {
        this.owner = owner;
        this.condition = condition;
        ParseFrom(jsonData);
    }

    public void CacheTarget(List<Character> targets)
    {
        m_cacheTargets = targets;
    }

    protected abstract void ParseFrom(JSONNode jsonData);
    public abstract SkillProcResult Proc(ActionContent content);

    public abstract List<Character> GetTargets(ActionContent content);

    internal void AddTri(string tri)
    {
        if (mLstTri == null)
        {
            mLstTri = new List<string>();
        }
        Debug.Log("t>>添加触发器:" + tri);//########
        mLstTri.Add(tri);
    }

    
    public bool IsTried(string tri)
    {
        if (mLstTri == null)
        {
            return false;
        }
        return mLstTri.Contains(tri);
    }

    /// <summary>
    /// 主动效果。无触发器
    /// </summary>
    /// <returns></returns>
    public bool IsActive()
    {
        return mLstTri == null || mLstTri.Count == 0;
    }
}
