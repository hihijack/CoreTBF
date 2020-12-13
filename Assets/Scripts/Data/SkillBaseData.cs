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

        public string icon;

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

        //施法距离
        public int distance;

        public string tip;

        public string tlAsset;
        public string tlAssetPower;

        public JSONNode data;

        public SkillBaseData(IDataReader reader)
        {
            ID = reader.GetInt16(0);
            name = reader.GetString(1);
            icon = reader.GetString(2);
            logic = (ESkillLogic)Enum.Parse(typeof(ESkillLogic), reader.GetString(3));
            targetType = (ESkillTarget)Enum.Parse(typeof(ESkillTarget), reader.GetString(4));
            dmg = reader.GetInt16(5) / 100f;
            dmgFire = reader.GetInt16(6) / 100f;
            dmgTenacity = reader.GetInt16(7);
            targetCount = reader.GetInt16(8);
            timePower = reader.GetFloat(9);
            backswing = reader.GetFloat(10);
            timeAtkStiff = reader.GetFloat(11);
            cost = reader.GetInt16(12);
            tenChangeTo = reader.GetFloat(13);
            tenChangeToPower = reader.GetFloat(14);
            distance = reader.GetInt16(15);
           
            tip = reader.GetString(16);
            tlAsset = reader.IsDBNull(17) ? "" : reader.GetString(17);
            tlAssetPower = reader.IsDBNull(18) ? "" : reader.GetString(18);
            data = reader.IsDBNull(19) ? null : JSONNode.Parse(reader.GetString(19));
        }
    }
}