using System;
using System.Collections.Generic;
using Data;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

static class AICfg
{
    public const string TYPE_ENEMY_TANK = "enemy_tank";
    public const string TYPE_RAN = "ran";
    public const string TYPE_ROLE = "role";
    public const string ROLE_ID = "role_id";
}

public class AI
{
    private readonly int[] arrSkillIndexToAction;
    int[] arrExrAISkillIndex;
    private readonly Character _character;

    private int _index;

    //Dictionary<Character, int> _dicHatredRecord;

    //Character _chrHatredTarget;//缓存仇恨最高目标,仇恨修改时计算一次.可能为空

    public AI(Character character)
    {
        _character = character;
        arrSkillIndexToAction = character.roleData.aiSkillIndexs;
        _index = -1;
        //_dicHatredRecord = new Dictionary<Character, int>();
    }

    /// <summary>
    /// 设置额外AI。优先使用额外AI
    /// </summary>
    /// <param name="arrAI"></param>
    internal void SetExrAI(int[] arrAI)
    {
        arrExrAISkillIndex = arrAI;
        _index = -1; //指向额外ai第一个
    }

    internal void RemoveExtAI()
    {
        arrExrAISkillIndex = null;
        _index = -1;
    }

    /// <summary>
    /// 记录仇恨
    /// </summary>
    //public void SetHatred(Character character, int hatred)
    //{
    //    if (_dicHatredRecord.ContainsKey(character))
    //    {
    //        _dicHatredRecord[character] = hatred;
    //    }
    //    else
    //    {
    //        _dicHatredRecord.Add(character, hatred);
    //    }
    //    SetHatredTarget();
    //}

    ///// <summary>
    ///// 添加仇恨值
    ///// </summary>
    ///// <param name="character"></param>
    ///// <param name="hatredOffset"></param>
    //public void AddHatred(Character character, int hatredOffset)
    //{
    //    if (_dicHatredRecord.ContainsKey(character))
    //    {
    //        _dicHatredRecord[character] = Mathf.Max(0, _dicHatredRecord[character] + hatredOffset);
    //    }
    //    else
    //    {
    //        _dicHatredRecord.Add(character, Mathf.Max(hatredOffset, 0));
    //    }
    //    SetHatredTarget();
    //}

    ///// <summary>
    ///// 预计算仇恨
    ///// </summary>
    ///// <param name="character"></param>
    ///// <param name="hatredOffset"></param>
    ///// <returns></returns>
    //public bool PreCalHatred(Character character, int hatredOffset)
    //{
    //    bool r = false;
    //    if (_chrHatredTarget != null && _character != _chrHatredTarget)
    //    {
    //        int curTargetHatred = GetHatred(_chrHatredTarget);
    //        int curHatred = GetHatred(character);
    //        if (curHatred + hatredOffset >= curTargetHatred)
    //        {
    //            r = true;
    //        }
    //    }

    //    return r;
    //}

    ///// <summary>
    ///// 获取仇恨值.默认为0
    ///// </summary>
    ///// <param name="character"></param>
    ///// <returns></returns>
    //public int GetHatred(Character character)
    //{
    //    _dicHatredRecord.TryGetValue(character, out int r);
    //    return r;
    //}


    /// <summary>
    /// 计算设置仇恨目标
    /// </summary>
    //void SetHatredTarget()
    //{
    //    Character chrTemp = null;
    //    int hatredTemp = 0;
    //    foreach (var chr in _dicHatredRecord.Keys)
    //    {
    //        if (_dicHatredRecord[chr] > hatredTemp)
    //        {
    //            chrTemp = chr;
    //            hatredTemp = _dicHatredRecord[chr];
    //        }
    //    }
    //    var t = _chrHatredTarget;
    //    _chrHatredTarget = chrTemp;
    //    if (t != _chrHatredTarget)
    //    {
    //        UIMgr.Inst.uiFightLog.AppendLog($"<Color=red>{_character.roleData.name}仇恨目标变更->{_chrHatredTarget.roleData.name}</Color>");
    //        UIMgr.Inst.uiFight.RefreshHatredTarget(_chrHatredTarget);
    //    }
    //}

    /// <summary>
    /// 取仇恨目标
    /// </summary>
    /// <returns></returns>
    //public Character GetHatredTarget()
    //{
    //    return _chrHatredTarget;
    //}

    /// <summary>
    /// 主要阶段要使用的行动
    /// </summary>
    /// <returns></returns>
    public FightActionBase ActionForMain()
    {
        Skill skill = null;
        if (_character.mSkillPowering != null)
        {
            //正在蓄力,只能使用蓄力技能
            skill = _character.mSkillPowering;
        }
        else
        {
            skill = GetNextSkill(_index, out _index);
        }

        //主动循环使用技能
        //TODO AI防御处理
        ActionContent content = FightActionFactory.Inst.CreateActionContent(_character, skill, GetSkillTargets(skill));
        return FightActionFactory.Inst.CreateFightAction(skill, content);
    }

    public struct DataNextSkillToCas
    {
        public SkillBaseData skillBaseData;
        public bool isPower; //发动蓄力技能
    }
    /// <summary>
    /// 即将使用的技能
    /// </summary>
    /// <returns></returns>
    public DataNextSkillToCas GetNextSkillToCast()
    {
        //蓄力技能
        if (_character.mSkillPowering != null)
        {
            return new DataNextSkillToCas() { skillBaseData = _character.mSkillPowering.GetBaseData(), isPower = true };
        }

        Skill skill = GetNextSkill(_index, out _);

        return new DataNextSkillToCas() { skillBaseData = skill.GetBaseData(), isPower = false };
    }

    /// <summary>
    /// 从index搜索下一个要释放的技能
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    Skill GetNextSkill(int index, out int newIndex)
    {
        var tempArr = arrExrAISkillIndex != null ? arrExrAISkillIndex : arrSkillIndexToAction;

        int t = index;
        newIndex = t;
        t++;
        if (t > tempArr.Length - 1)
        {
            t = 0;
        }

        int maxSearchTime = tempArr.Length;

        Skill skill = null;
        while (skill == null)
        {
            skill = _character.lstSkill[tempArr[t] - 1];
            if (!skill.IsTimeLimitEnable())
            {
                skill = null;
                t++;
                if (t > tempArr.Length - 1)
                {
                    t = 0;
                }
            }
            else
            {
                //找到合适的技能
                newIndex = t;
                break;
            }

            //最大搜索次数
            maxSearchTime--;
            if (maxSearchTime <= 0)
            {
                break;
            }
        }
        return skill;
    }

    /// <summary>
    /// TOOD 速攻反击阶段要使用的行动
    /// </summary>
    /// <returns></returns>
    public FightActionBase ActionForQuick()
    {
        //TODO 速攻反击阶段要使用的行动
        return null;
    }

    List<Character> GetSkillTargets(Skill skill)
    {
        var skillData = skill.GetBaseData();

       

        //TODO AI目标获取
        //if (skillData.targetType == ESkillTarget.Enemy)
        //{
        //    //从允许目标中找到第一个存活的
        //    Character targetChara = null;
        //    //以1号位为目标
        //    int targetTeamLoc = 0;
        //    //查找正在蓄力的敌方目标
        //    var poweringChara = FightState.Inst.characterMgr.FindThatIsPowering(ECamp.Ally);
        //    if (poweringChara != null)
        //    {
        //        targetChara = poweringChara;
        //        targetTeamLoc = targetChara.teamLoc;
        //    }
        //    else
        //    {
        //        targetTeamLoc = 1;
        //        targetChara = FightState.Inst.characterMgr.GetAliveCharacter(ECamp.Ally, targetTeamLoc);
        //    }

        //    targets.Add(targetChara);
        //    for (int i = 0; i < skillData.targetCount - 1; i++)
        //    {
        //       //往后添加目标
        //        var chara = FightState.Inst.characterMgr.GetAliveCharacter(ECamp.Ally, targetTeamLoc + i + 1);
        //        if (chara != null)
        //        {
        //            targets.Add(chara);
        //        }
        //    }
        //}

        List<Character> targets = new List<Character>();

        if (!skill.GetBaseData().IsNeedSelectTarget())
        {
            return targets;
        }

        var targetsEnable = FightState.Inst.characterMgr.GetSkillTargets(skillData, _character);
        //取目标
        if (targetsEnable.Count > 0)
        {
            Character targetChara = null;
            string type = skillData.ai["type"].Value;
            if (!string.IsNullOrEmpty(type))
            {
                if (type == AICfg.TYPE_ENEMY_TANK)
                {
                    //取敌方坦克
                    targetChara = GetEnemyTank(targetsEnable);
                }
                else if (type == AICfg.TYPE_RAN)
                {
                    //随机取目标
                    int ranIndex = Random.Range(0, targetsEnable.Count);
                    targetChara = targetsEnable[ranIndex];
                }
                else if (type == AICfg.TYPE_ROLE)
                {
                    //指定目标
                    int roleID = skillData.ai["role_id"].AsInt;
                    targetChara = GetTargetWithRoleID(targetsEnable, roleID);
                    if (targetChara == null)
                    {
                        Debug.LogError("AI指定角色ID查找失败,roleID:" + roleID);
                    }
                }
                else
                {
                    Debug.LogError("无效的AI目标模式" + type);
                }
            }
            if (targetChara != null)
            {
                Debug.Log($"AI目标选择,{_character.roleData.name},{skill.GetBaseData().name}->{targetChara.roleData.name}");
                //找到目标
                targets.Add(targetChara);
                var targetTeamLoc = targetChara.teamLoc;
                //多目标取其他目标
                for (int i = 0; i < skillData.targetCount - 1; i++)
                {
                    //往后添加目标
                    var chara = FightState.Inst.characterMgr.GetAliveCharacter(ECamp.Ally, targetTeamLoc + i + 1);
                    if (chara != null)
                    {
                        targets.Add(chara);
                    }
                }
            }
            else
            {
                //没找到目标
            }
        }

        if (targets.Count == 0)
        {
            Debug.LogError("空技能目标:" + skill.GetBaseData().name);
        }

        return targets;
    }

    /// <summary>
    /// 查找指定ID
    /// </summary>
    /// <param name="targets"></param>
    /// <param name="roleID"></param>
    /// <returns></returns>
    private Character GetTargetWithRoleID(List<Character> targets, int roleID)
    {
        Character targetChara = null;
        foreach (var charaT in targets)
        {
            if (charaT.roleData.ID == roleID)
            {
                targetChara = charaT;
                break;
            }
        }
        return targetChara;
    }

    /// <summary>
    /// 查找敌方阵营的坦克
    /// </summary>
    /// <param name="targets"></param>
    /// <returns></returns>
    private Character GetEnemyTank(List<Character> targets)
    {
        Character targetChara = null;
        foreach (var charaT in targets)
        {
            if (charaT.camp == _character.GetEnemyCamp())
            {
                if (targetChara == null)
                {
                    targetChara = charaT;
                }
                else
                {
                    if (charaT.teamLoc < targetChara.teamLoc)
                    {
                        targetChara = charaT;
                    }
                }
            }
        }
        return targetChara;
    }

    /// <summary>
    /// 取硬直最小的一个敌方角色
    /// </summary>
    /// <returns></returns>
    //Character GetNearestEnemyCharacter()
    //{
    //    Character r = null;
    //    ECamp enemy = _character.GetEnemyCamp();
    //    foreach (var t in GameMgr.Inst.lstCharacters)
    //    {
    //        if (t.camp == enemy && t.propData.hp > 0)
    //        {
    //            if ( r == null || t.mTimeStiff < r.mTimeStiff)
    //            {
    //                r = t;
    //            }
    //        }
    //    }
    //    return r;
    //}
}