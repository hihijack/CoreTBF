using System;
using System.Collections.Generic;
using Data;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class AI
    {
        private readonly int[] arrSkillIndexToAction;
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
                _index++;
                if (_index > arrSkillIndexToAction.Length - 1)
                {
                    _index = 0;
                }
                skill = _character.lstSkill[arrSkillIndexToAction[_index] - 1];
            }

            //主动循环使用技能
            //TODO AI防御处理
            ActionContent content = FightActionFactory.Inst.CreateActionContent(_character, skill, GetSkillTargets(skill));
            return FightActionFactory.Inst.CreateFightAction(skill, content);
        }

        /// <summary>
        /// 即将使用的技能
        /// </summary>
        /// <returns></returns>
        public SkillBaseData GetNextSkillToCast()
        {
            int t = _index;
            t++;
            if (t > arrSkillIndexToAction.Length - 1)
            {
                t = 0;
            }
            var skill = _character.lstSkill[arrSkillIndexToAction[t] - 1];
            return skill.GetBaseData();
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

            var targets = new List<Character>();

            //TODO AI目标获取
            if (skillData.targetType == ESkillTarget.Enemy)
            {
                //从允许目标中找到第一个存活的
                Character targetChara = null;
                //以1号位为目标
                int targetTeamLoc = 0;
                //查找正在蓄力的敌方目标
                var poweringChara = FightState.Inst.characterMgr.FindThatIsPowering(ECamp.Ally);
                if (poweringChara != null)
                {
                    targetChara = poweringChara;
                    targetTeamLoc = targetChara.teamLoc;
                }
                else
                {
                    targetTeamLoc = 1;
                    targetChara = FightState.Inst.characterMgr.GetAliveCharacter(ECamp.Ally, targetTeamLoc);
                }

                targets.Add(targetChara);
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

            return targets;
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
}