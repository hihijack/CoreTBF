﻿using System;
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

        Dictionary<Character, int> _dicHatredRecord;

        Character _chrHatredTarget;//缓存仇恨最高目标,仇恨修改时计算一次.可能为空

        public AI(Character character)
        {
            _character = character;
            arrSkillIndexToAction = new int[]{0,0,1,2};
            _index = -1;
            _dicHatredRecord = new Dictionary<Character, int>();
        }

        /// <summary>
        /// 记录仇恨
        /// </summary>
        public void SetHatred(Character character, int hatred)
        {
            if (_dicHatredRecord.ContainsKey(character))
            {
                _dicHatredRecord[character] = hatred;
            }
            else
            {
                _dicHatredRecord.Add(character, hatred);
            }
            SetHatredTarget();
        }

        /// <summary>
        /// 添加仇恨值
        /// </summary>
        /// <param name="character"></param>
        /// <param name="hatredOffset"></param>
        public void AddHatred(Character character, int hatredOffset)
        {
            if (_dicHatredRecord.ContainsKey(character))
            {
                _dicHatredRecord[character] = Mathf.Max(0, _dicHatredRecord[character] + hatredOffset);
            }
            else
            {
                _dicHatredRecord.Add(character, Mathf.Max(hatredOffset, 0));
            }
            SetHatredTarget();
        }

        /// <summary>
        /// 获取仇恨值.默认为0
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public int GetHatred(Character character)
        {
            _dicHatredRecord.TryGetValue(character, out int r);
            return r;
        }


        /// <summary>
        /// 计算设置仇恨目标
        /// </summary>
        void SetHatredTarget()
        {
            Character chrTemp = null;
            int hatredTemp = 0;
            foreach (var chr in _dicHatredRecord.Keys)
            {
                if (_dicHatredRecord[chr] > hatredTemp)
                {
                    chrTemp = chr;
                    hatredTemp = _dicHatredRecord[chr];
                }
            }
            var t = _chrHatredTarget;
            _chrHatredTarget = chrTemp;
            if (t != _chrHatredTarget)
            {
                UIMgr.Inst.uiFightLog.AppendLog($"{_character.roleData.name}仇恨目标变更->{_chrHatredTarget.roleData.name}");
            }
        }

        /// <summary>
        /// 取仇恨目标
        /// </summary>
        /// <returns></returns>
        public Character GetHatredTarget()
        {
            return _chrHatredTarget;
        }

        /// <summary>
        /// 主要阶段要使用的行动
        /// </summary>
        /// <returns></returns>
        public FightActionBase ActionForMain()
        {
            _index++;
            if (_index > arrSkillIndexToAction.Length - 1)
            {
                _index = 0;
            }
            //主动循环使用技能
            //TODO AI防御处理
            var skillData = _character.lstSkillData[arrSkillIndexToAction[_index]];
            return FightActionFactory.Inst.CreateFightAction(_character, skillData, GetSkillTargets(skillData));
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

        List<Character> GetSkillTargets(SkillData skillData)
        {
            var targets = new List<Character>();
            if (skillData.targetCount == 1)
            {
                if (skillData.targetType == ESkillTarget.Enemy)
                {
                    if (_chrHatredTarget != null)
                    {
                        targets.Add(_chrHatredTarget);
                    }
                    else
                    {
                        targets.Add(GetNearestEnemyCharacter());
                    }
                }else if (skillData.targetType == ESkillTarget.Ally)
                {
                    targets = GetRandomOfCamp(1, _character.camp);
                }else if (skillData.targetType == ESkillTarget.Self)
                {
                    targets.Add(_character);
                }
               
            }else if (skillData.targetCount > 1)
            {
                if (skillData.targetType == ESkillTarget.Enemy)
                {
                    targets = GetRandomOfCamp(skillData.targetCount, _character.GetEnemyCamp());
                }else if (skillData.targetType == ESkillTarget.Ally)
                {
                    targets = GetRandomOfCamp(skillData.targetCount, _character.camp);
                }else if (skillData.targetType == ESkillTarget.Self)
                {
                    targets.Add(_character);
                }
            }

            return targets;
        }

        /// <summary>
        /// 取硬直最小的一个敌方角色
        /// </summary>
        /// <returns></returns>
        Character GetNearestEnemyCharacter()
        {
            Character r = null;
            ECamp enemy = _character.GetEnemyCamp();
            foreach (var t in GameMgr.Inst.lstCharacters)
            {
                if (t.camp == enemy && t.propData.hp > 0)
                {
                    if ( r == null || t.mTimeStiff < r.mTimeStiff)
                    {
                        r = t;
                    }
                }
            }
            return r;
        }

        /// <summary>
        /// 取随机N个敌方
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        List<Character> GetRandomOfCamp(int count, ECamp camp)
        {
            if (count == 0)
            {
                return null;
            }

            List<Character> r = new List<Character>();
            //先加入所有活着的敌人
            foreach (var t in GameMgr.Inst.lstCharacters)
            {
                if (t.propData.hp > 0 && t.camp == camp)
                {
                    r.Add(t);
                }
            }

            int countToRemove = r.Count - count;//需要移除的数量
            if (countToRemove > 0)
            {
                for (int i = 0; i < countToRemove; i++)
                {
                    //移除随机一个
                    int indexToRemove = Random.Range(0, r.Count);
                    r.RemoveAt(indexToRemove);
                }
            }
            
            return r;
        }
    }
}