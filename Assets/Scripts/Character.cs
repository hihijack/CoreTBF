using System;
using System.Collections.Generic;
using Data;
using UI;
using UnityEngine;
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
        Acting //正在行动,非等待或防御或蓄力
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

        public ECharacterState State
        {
            get
            {
                return state;
            }

            set
            {
                state = value;
                Debug.Log("set state:" + value + "," + roleData.name);//######
            }
        }

        public Character(int dataID)
        {
            roleData = GameData.Inst.roleData.Get(dataID);
            var pfb = UnityEngine.Resources.Load<GameObject>($"Prefabs/Character/{roleData.model}");
            if (!pfb) return;
            var go = Object.Instantiate(pfb);
            entityCtl = GameUtil.GetOrAdd<RoleEntityCtl>(go);
            entityCtl.Init(roleData);

            mTimeStiff = 5 / roleData.speed;
            State = ECharacterState.Stiff;
            
            lstSkillData = new List<SkillData>(4);
            foreach (var skillID in roleData.skills)
            {
                var skillData = GameData.Inst.skillData.Get(skillID);
                if (skillData.quick)
                {
                    _hasQuickSkill = true;
                }
                lstSkillData.Add(skillData);  
            }

            propData = new PropData();
            propData.maxHP = roleData.HP;
            propData.maxMP = roleData.MP;
            propData.hp = propData.maxHP;
            propData.mp = propData.maxMP;
            propData.atk = roleData.atk;
            propData.def = roleData.def;
            
            //AI
            ai = new AI(this);
        }

        /// <summary>
        /// 正常状态更新    
        /// </summary>
        public void UpdateInNormalStage()
        {
            var oriTimeStiff = mTimeStiff;
            mTimeStiff -= Time.deltaTime;
            if (mTimeStiff <= 0)
            {
                mTimeStiff = 0;
            }

            if (State == ECharacterState.Power)
            {
                mTimePower += Time.deltaTime;
            }
            
            if (oriTimeStiff > 0 && mTimeStiff <= 0)
            {
                State = ECharacterState.Active;
                //进入触发
                GameMgr.Inst.SetActiveCharacte(this);
            }
        }

        public bool IsInReady()
        {
            return mTimeStiff <= 0;
        }

        public void ActionSelectPoweringSkill()
        {
            OnActionSelected(mSkillPowering);
        }
        
        public void OnActionSelected(SkillData skillData)
        {
            //计算技能目标
            var targets = new List<Character>();
            if (skillData.targetCount == 1)
            {
                targets.Add(target);
            }else if (skillData.targetCount > 1)
            {
                targets = GameMgr.Inst.GetCharactersOfCamp(GetEnemyCamp());
            }
            
            GameMgr.Inst.CaheAction(FightActionFactory.Inst.CreateFightAction(this, skillData, targets));
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

        public void DamageTarget(Character target, float skillDmg, float timeAtkStiff)
        {
            int dmg = CalDmg(skillDmg);

            //防御
            if (target.State == ECharacterState.Def)
            {
                dmg = 0;
                timeAtkStiff *= 0.5f;
            }
            else if (target.State == ECharacterState.Power)
            {
                //TODO 蓄力打断

            }

            if (target.camp == ECamp.Ally)
            {
               PlayerRolePropDataMgr.Inst.ChangeHP(-1 * dmg);
            }
            else if (target.camp == ECamp.Enemy)
            {
                target.propData.ChangeHP(-1 * dmg);
                UIEnemyPlayerInfo.Inst.Refresh();
            }
            //硬直处理
            target.mTimeStiff = Mathf.Max(timeAtkStiff, target.mTimeStiff);
            target.State = ECharacterState.Stiff;
            target.mTimePower = 0f;
            target.mSkillPowering = null;


            UIMgr.Inst.uiFightLog.AppendLog($"{roleData.name}对{target.roleData.name}造成了{dmg}点伤害!");
        }

        private int CalDmg(float skillDmg)
        {
            return Mathf.CeilToInt(skillDmg * propData.atk);
        }

        internal bool IsAlive()
        {
            return propData.hp > 0;
        }
    }
}