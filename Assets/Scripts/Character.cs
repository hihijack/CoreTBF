﻿using System;
using System.Collections.Generic;
using Data;
using Sirenix.Serialization;
using UI;
using UnityEngine;
using UnityEngine.Accessibility;
using Object = UnityEngine.Object;

namespace DefaultNamespace
{
    public enum ECamp
    {
        Ally,
        Enemy
    }

    public enum ECharacterState
    {
        Stiff,
        Active,//当前进入触发
        Wait,
        Def,
        Power,
        Acting, //正在行动,非等待或防御或蓄力
        Dying, //濒死
        Dead //完全死亡
    }
    
    public struct DmgData
    {
        public float dmgPrecent;//伤害百分百
        public float timeAtkStiff;//攻击硬直
        public int tenAtk;//韧性伤害
    }

    public class Character
    {
        public RoleEntityCtl entityCtl;
        public RoleData roleData;
        public PropData propData;
        public List<SkillData> lstSkillData;
        public int teamLoc;
        public ECamp camp;

        private ECharacterState state;

        public float mTimeStiff;

        public float mTimePower;

        public SkillData mSkillPowering;//正在蓄力的技能
        
        private bool _hasQuickSkill = false;

        public Character target;//仇恨目标

        public AI ai;

        public List<BuffBase> lstBuffs;

        public ECharacterState State
        {
            get
            {
                return state;
            }

            set
            {
                state = value;
            }
        }

        public Character(int dataID)
        {
            roleData = GameData.Inst.roleData.Get(dataID);
            var pfb = UnityEngine.Resources.Load<GameObject>($"Prefabs/Character/{roleData.model}");
            if (!pfb) return;
            var go = Object.Instantiate(pfb);
            entityCtl = GameUtil.GetOrAdd<RoleEntityCtl>(go);
            entityCtl.Init(this);
            entityCtl.SetSprite("idle");

            mTimeStiff = 5 / roleData.speed;
            State = ECharacterState.Stiff;
            
            lstSkillData = new List<SkillData>(4);
            foreach (var skillID in roleData.skills)
            {
                var skillData = GameData.Inst.skillData.Get(skillID);
                //if (skillData.quick)
                //{
                //    _hasQuickSkill = true;
                //}
                lstSkillData.Add(skillData);  
            }

            propData = new PropData();
            propData.MaxHP = roleData.HP;
            propData.maxMP = roleData.MP;
            propData.hp = propData.MaxHP;
            propData.mp = propData.maxMP;
            propData.atk = roleData.atk;
            propData.def = roleData.def;
            propData.tenacityMax = roleData.tenacity;
            propData.tenacity = propData.tenacityMax;
           
            //AI
            ai = new AI(this);

            lstBuffs = new List<BuffBase>(10);
        }

        /// <summary>
        /// 添加一个buff.如果已存在相同,且可以叠加,则叠加一层,并刷新持续时间
        /// </summary>
        /// <param name="buffID"></param>
        /// <param name="caster"></param>
        public void AddABuff(int buffID, Character caster)
        {
            var buff = GetBuff(buffID);
            if (buff != null)
            {
                if (buff.GetBuffData().maxLayer > 1)
                {
                    //叠加
                    buff.ChangeLayer(1);
                }
                //刷新时间
                buff.RestartDur();
            }
            else
            {
                //新增buff
                buff = BuffFactory.CreateABuff(buffID, this, caster);
                lstBuffs.Add(buff);
                buff.OnAdd();
                UIMgr.Inst.uiFight.RefreshBuffUIOnAdd(this, buff);
            }
        }

        public BuffBase GetBuff(int id)
        {
            foreach (var t in lstBuffs)
            {
                if (t.GetBuffData().ID == id && t.IsValid())
                {
                    return t;
                }
            }
            return null;
        }

        /// <summary>
        /// 正常状态更新    
        /// </summary>
        public void UpdateInNormalStage()
        {
            if (!IsAlive())
            {
                //死亡
                entityCtl.SetPos(Vector3.right * 1000);
            }

            var oriTimeStiff = mTimeStiff;
            mTimeStiff -= Time.deltaTime;
            mTimeStiff = Mathf.Max(mTimeStiff, 0);

            if (State == ECharacterState.Power)
            {
                mTimePower += Time.deltaTime;
            }

            //更新buff
            RefreshBuff();

            if (IsEnableAction)
            {
                if (oriTimeStiff > 0 && mTimeStiff <= 0)
                {
                    ActiveAction();
                }

                //等待恢复MP
                if (State == ECharacterState.Wait)
                {
                    if (Time.frameCount % 60 == 0)
                    {
                        PlayerRolePropDataMgr.Inst.ChangeMP(5);
                    }
                }
            }

            //更新图片
            if (State == ECharacterState.Dying)
            {
                entityCtl.SetSprite("dying");
            }
            else if (State == ECharacterState.Def)
            {
                entityCtl.SetSprite("def");
            }
            else if (State == ECharacterState.Power)
            {
                entityCtl.SetSprite("power");
            }
            else
            {
                entityCtl.SetSprite("idle");
            }
        }

        void RefreshBuff()
        {
            foreach (var buff in lstBuffs)
            {
                buff.UpdateDur(Time.deltaTime);
            }

            for (int i = lstBuffs.Count - 1; i >= 0; i--)
            {
                if (!lstBuffs[i].IsValid())
                {
                    UIMgr.Inst.uiFight.RefreshBuffUIOnRemove(this, lstBuffs[i]);
                    lstBuffs.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 拥有行动能力,可以行动
        /// </summary>
        /// <returns></returns>
        public bool IsEnableAction
        {
            get
            {
                return State != ECharacterState.Dying && State != ECharacterState.Dead;
            }
        }

        /// <summary>
        /// 进入触发
        /// </summary>
        public void ActiveAction()
        {
            State = ECharacterState.Active;
            //从break中恢复
            var buffBreak = GetBuff(2);
            if (buffBreak != null && buffBreak.IsValid())
            {
                buffBreak.SetLayer(0);
                RefreshBuff();
                propData.RecoverTenacity();
                UIMgr.Inst.uiHPRoot.RefreshTarget(this);
            }

            //进入触发
            GameMgr.Inst.SetActiveCharacte(this);
        }

        public bool IsInReady()
        {
            return mTimeStiff <= 0;
        }

        public void ActionSelectPoweringSkill(Character target)
        {
            OnActionSelected(mSkillPowering, target);
        }
        
        public void OnActionSelected(SkillData skillData, Character target)
        {
            //计算技能目标
            var targets = new List<Character>();
            if (skillData.targetCount == 1)
            {
                targets.Add(target);
            }
            else if (skillData.targetCount > 1)
            {
                targets = GameMgr.Inst.GetCharactersOfCamp(GetEnemyCamp());
            }
            
            GameMgr.Inst.CaheAction(FightActionFactory.Inst.CreateFightAction(this, skillData, targets));
            GameMgr.Inst.ToNextStage();
        }

        public bool HasQuickSkill()
        {
            return _hasQuickSkill;
        }

        public ECamp GetEnemyCamp()
        {
            switch (camp)
            {
               case ECamp.Ally:
                   return ECamp.Enemy;
               case ECamp.Enemy:
                   return ECamp.Ally;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void DamageTarget(Character target, DmgData dmgData)
        {

            int dmg = CalDmg(dmgData.dmgPrecent);

            //计算增伤减伤
            dmg = Mathf.CeilToInt(dmg * target.propData.DmgHurtedMul);

            if (target.propData.shield > 0)
            {
                //消耗护盾抵消伤害
                target.propData.ChangeShield(-1);
                dmg = 0;
            }

            //防御抵挡伤害
            if (target.State == ECharacterState.Def)
            {
                dmg = 0;
            }

            //计算抗性
            var targetDef = (float)target.propData.Def;
            dmg = Mathf.CeilToInt(dmg * (1 - targetDef * 6 / (100 + targetDef * 6)));

            UIMgr.Inst.uiFightLog.AppendLog($"{roleData.name}对{target.roleData.name}造成了{dmg}点伤害!");
            target.Hurted(dmg);

            if (target.IsEnableAction)
            {
                //硬直处理
                target.mTimeStiff = Mathf.Max(dmgData.timeAtkStiff, target.mTimeStiff);
                //削韧硬直
                int oriTenacity = target.propData.tenacity;
                target.propData.ChangeTenacity(-1 * CalTenacityDmg(dmgData.tenAtk, target.propData));
                if (oriTenacity > 0 && target.propData.tenacity <= 0)
                {
                    //韧性清空
                    target.OnTenacityCleared();
                }
            }

            UIMgr.Inst.uiHPRoot.RefreshTarget(target);
        }

        /// <summary>
        /// 计算韧性伤害
        /// </summary>
        /// <param name="tenAtk"></param>
        /// <param name="propDataTarget"></param>
        /// <returns></returns>
        private int CalTenacityDmg(int tenAtk, PropData propDataTarget)
        {
            return Mathf.CeilToInt(tenAtk *  (100 - propDataTarget.Toughness) / 100f);
        }

        /// <summary>
        /// 受伤
        /// </summary>
        /// <param name="dmg"></param>
        private void Hurted(int dmg)
        {
            propData.ChangeHP(-1 * dmg);

            //死亡处理
            if (propData.hp <= 0)
            {
                if (State != ECharacterState.Dying && State != ECharacterState.Dead)
                {
                    //进入濒死
                    ToDying();
                }
                else if (State == ECharacterState.Dying)
                {
                    //完全死亡
                    ToDead();
                }
            }
        }

        /// <summary>
        /// 完全死亡
        /// </summary>
        private void ToDead()
        {
            UIMgr.Inst.uiFightLog.AppendLog($"<Color=red>{roleData.name}死亡!</Color>");
            State = ECharacterState.Dead;
            //硬直清空
            mTimeStiff = 0f;
            mSkillPowering = null;
            mTimePower = 0f;
            //清空buff,所有buff层数归0
            ClearAllBuff();
            GameMgr.Inst.OnCharacterDead(this);
        }

        /// <summary>
        /// 清空所有buff
        /// </summary>
        private void ClearAllBuff()
        {
            foreach (var buff in lstBuffs)
            {
                buff.SetLayer(0);
            }
        }

        /// <summary>
        /// 濒死
        /// </summary>
        private void ToDying()
        {
            UIMgr.Inst.uiFightLog.AppendLog($"<Color=red>{roleData.name}已经濒死!失去行动能力</Color>");
            State = ECharacterState.Dying;
            //硬直清空
            mTimeStiff = 0f;
            mTimePower = 0f;
            mSkillPowering = null;
            //hp上限为50%,并恢复满血
            propData.MaxHPParamMul -= 0.5f;
            propData.hp = propData.MaxHP;
        }

        /// <summary>
        /// 当韧性被清空
        /// </summary>
        private void OnTenacityCleared()
        {
            //中断蓄力
            State = ECharacterState.Stiff;
            mTimePower = 0f;
            mSkillPowering = null;
            //硬直时间
            mTimeStiff += roleData.tenClearStiff;

            //添加BREAK BUFF
            AddABuff(2, null);
            //propData.RecoverTenacity();
            UIMgr.Inst.uiHPRoot.RefreshTarget(this);
        }

        private int CalDmg(float skillDmg)
        {
            return Mathf.CeilToInt(skillDmg * propData.atk);
        }

        /// <summary>
        /// 濒死也算存活
        /// </summary>
        /// <returns></returns>
        internal bool IsAlive()
        {
            return State != ECharacterState.Dead;
        }
    }
}