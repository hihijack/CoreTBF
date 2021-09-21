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
    public const string HEAL_TARGET = "heal_target";//治疗目标
    public const string ADD_BUFF = "add_buff";
    public const string REMOVE_BUFF = "remove_buff";//移除buff
    public const string SUMMON = "summon";
    public const string GET_MP = "get_mp";
    public const string CHANGE_PROP = "change_prop";
    public const string CHANGE_LOC = "change_loc";
    public const string CHANGE_AI = "change_ai";
    public const string KILL = "kill";
    public const string CONDITION_NONE = "none";
}

public static class FightSkillConditionVal
{
    public const string HP_LINE = "hp_line";
    public const string BUFF_LAYER = "buff_layer";
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
    public const string ACTIVE = "active"; //生效时；技能拥有；buff拥有时
    public const string BEFORE_END_TRUN = "end_turn";//回合结束前
    public const string LAYER_CHANGE = "layer_change";//buff层数改变
}

/// <summary>
/// 技能效果目标
/// </summary>
public static class SkillProcTarget
{
    public const string Targets = "target"; //所有目标
    public const string Self = "self"; //自身
    public const string RanTarget = "rantarget"; //随机目标
    public const string Tank = "tank"; //敌方最前排单位
}

/// <summary>
/// 换位类型
/// </summary>
public static class LocChangeType
{
    public const string AHEAD = "ahead";
    public const string LAST = "last";
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

    SkillProcTargetFinderBase targetFinder;

    public FightSkillProcessorBase(ISkillProcOwner owner, JSONNode jsonData, FightSkillConditionBase condition)
    {
        this.owner = owner;
        this.condition = condition;
        if (this.condition != null)
        {
            this.condition.SetOwnerProc(this);
        }
        ParseFrom(jsonData);
        ParseFinder(jsonData);
    }

    /// <summary>
    /// 构建目标选择器
    /// </summary>
    /// <param name="jsonData"></param>
    private void ParseFinder(JSONNode jsonData)
    {
        string targetType = jsonData["target"].Value;
        if (!string.IsNullOrEmpty(targetType))
        {
            switch (targetType)
            {
                case SkillProcTarget.Targets:
                    targetFinder = new TargetFinderTargets();
                    break;
                case SkillProcTarget.Self:
                    targetFinder = new TargetFinderSelf(owner);
                    break;
                case SkillProcTarget.RanTarget:
                    targetFinder = new TargetFinderRanTarget(owner);
                    break;
                case SkillProcTarget.Tank:
                    //敌方最前排目标
                    targetFinder = new TargetFinderEnemyTank(owner);
                    break;
                default:
                    break;
            }
        }
        else
        {
            var targetRole = jsonData["target_role"];
            if (targetRole != null)
            {
                var roleID = targetRole.AsInt;
                targetFinder = new TargetFinderRoleID(roleID);
            }
        }
    }

    public void CacheTarget(List<Character> targets)
    {
        m_cacheTargets = targets;
    }

    public bool CheckConditon()
    {
        return condition.IsTrue();
    }

    protected abstract void ParseFrom(JSONNode jsonData);
    public abstract SkillProcResult Proc(ActionContent content);

    public virtual List<Character> GetTargets(ActionContent content) 
    {
        if (m_cacheTargets != null)
        {
            return m_cacheTargets;
        }

        List<Character> targets = targetFinder.GetTargets(content);
        return targets;
    }

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
