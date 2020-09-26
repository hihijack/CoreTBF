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
        Ally,
        Enemy,
        Self
    }
    
    public class SkillBaseData
    {
        public int ID;
        public string name;

        /// <summary>
        /// 逻辑
        /// </summary>
        public ESkillLogic logic;

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

        //技能目标;{1,2,3}
        public JSONNode targetTeamLocs;

        public string tip;

        public string tlAsset;
        public string tlAssetPower;

        public JSONNode data;

        public SkillBaseData(IDataReader reader)
        {
            ID = reader.GetInt16(0);
            name = reader.GetString(1);
            logic = (ESkillLogic)Enum.Parse(typeof(ESkillLogic), reader.GetString(2));
            targetType = (ESkillTarget)Enum.Parse(typeof(ESkillTarget), reader.GetString(3));
            dmg = reader.GetInt16(4);
            dmgFire = reader.GetInt16(5);
            dmgTenacity = reader.GetInt16(6);
            targetCount = reader.GetInt16(7);
            timePower = reader.GetFloat(8);
            backswing = reader.GetFloat(9);
            timeAtkStiff = reader.GetFloat(10);
            cost = reader.GetInt16(11);
            tenChangeTo = reader.GetFloat(12);
            tenChangeToPower = reader.GetFloat(13);
            targetTeamLocs = JSONNode.Parse(reader.GetString(14));
            tip = reader.GetString(15);
            tlAsset = reader.IsDBNull(16) ? "" : reader.GetString(16);
            tlAssetPower = reader.IsDBNull(17) ? "" : reader.GetString(17);
            data = reader.IsDBNull(18) ? null : JSONNode.Parse(reader.GetString(18));
        }
    }
}