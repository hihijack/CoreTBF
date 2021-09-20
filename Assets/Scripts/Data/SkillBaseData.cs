using SimpleJSON;
using Sirenix.OdinInspector;
using System;
using System.Data;
using UnityEngine;
using UnityEngine.Timeline;

namespace Data
{
    public enum ESkillLogic
    {
        Wait,
        Def,
        Atk,
        ExchangeLoc
    }

    public enum ESkillTarget
    {
        None, //不选取目标
        Ally,
        Enemy,
        Self,
        SelfOrAlly
    }
    
    public enum ESkillType
    {
        Active, //主动技能
        Passive, //被动技能
        Temp //触发马甲技能
    }

    public class SkillBaseData
    {
        public int ID;
        public string name;

        public ESkillType type;

        public string icon;

        /// <summary>
        /// 逻辑
        /// </summary>
        public ESkillLogic logic;

        public int timeLimit;

        /// <summary>
        /// 目标类型
        /// </summary>
        public ESkillTarget targetType;

        /// <summary>
        /// 伤害
        /// </summary>
        public float dmg,dmgFire;

        /// <summary>
        /// 目标数量
        /// </summary>
        public int targetCount;

        /// <summary>
        /// 蓄力时间
        /// </summary>
        public float timePower;
        /// <summary>
        /// 后摇
        /// </summary>
        public float backswing;
        /// <summary>
        /// 目标硬直
        /// </summary>
        public float timeAtkStiff;
        /// <summary>
        /// 削韧
        /// </summary>
        public int dmgTenacity;
        public int cost;
        public int mpGet;

        /// <summary>
        /// 韧性变化至指定百分比
        /// </summary>
        public float tenChangeTo;

        /// <summary>
        /// 开始蓄力时的韧性变化
        /// </summary>
        public float tenChangeToPower;

        //施法距离
        public int distance;

        public string tip;

        public string tlAsset;
        public string tlAssetPower;

        public JSONNode data;

        public JSONNode ai;

        /// <summary>
        /// 允许的职业ID
        /// </summary>
        public int[] enableJob;

        public SkillBaseData(IDataReader reader)
        {
            ID = reader.GetInt16(0);
            name = reader.GetString(1);
            type = (ESkillType)Enum.Parse(typeof(ESkillType), reader.GetString(2));
            icon = reader.GetString(3);
            logic = (ESkillLogic)Enum.Parse(typeof(ESkillLogic), reader.GetString(4));
            targetType = (ESkillTarget)Enum.Parse(typeof(ESkillTarget), reader.GetString(5));
            timeLimit = reader.GetInt16(6);
            dmg = reader.GetInt16(7) / 100f;
            dmgFire = reader.GetInt16(8) / 100f;
            dmgTenacity = reader.GetInt16(9);
            targetCount = reader.GetInt16(10);
            timePower = reader.GetFloat(11);
            backswing = reader.GetFloat(12);
            timeAtkStiff = reader.GetFloat(13);
            cost = reader.GetInt16(14);
            tenChangeTo = reader.GetFloat(15);
            tenChangeToPower = reader.GetFloat(16);
            distance = reader.GetInt16(17);
           
            tip = reader.GetString(18);
            tlAsset = reader.IsDBNull(19) ? "" : reader.GetString(19);
            tlAssetPower = reader.IsDBNull(20) ? "" : reader.GetString(20);
            data = reader.IsDBNull(21) ? null : JSONNode.Parse(reader.GetString(21));

            ai = reader.IsDBNull(22) ? null : JSONNode.Parse(reader.GetString(22));

            JSONNode jsonJob = reader.IsDBNull(23) ? null : JSONNode.Parse(reader.GetString(23));
            if (jsonJob != null)
            {
                enableJob = jsonJob.AsArray.ToIntArr();
            }
        }

        public bool IsTargetTypeContainAlly()
        {
            return targetType == ESkillTarget.Ally || targetType == ESkillTarget.SelfOrAlly;
        }

        public bool IsTargetTypeContainSelf()
        {
            return targetType == ESkillTarget.Self || targetType == ESkillTarget.SelfOrAlly;
        }

        public bool IsNeedSelectTarget()
        {
            return targetType != ESkillTarget.None;
        }
    }
}