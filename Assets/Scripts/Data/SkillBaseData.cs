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
            dmg = reader.GetInt16(6) / 100f;
            dmgFire = reader.GetInt16(7) / 100f;
            dmgTenacity = reader.GetInt16(8);
            targetCount = reader.GetInt16(9);
            timePower = reader.GetFloat(10);
            backswing = reader.GetFloat(11);
            timeAtkStiff = reader.GetFloat(12);
            cost = reader.GetInt16(13);
            tenChangeTo = reader.GetFloat(14);
            tenChangeToPower = reader.GetFloat(15);
            distance = reader.GetInt16(16);
           
            tip = reader.GetString(17);
            tlAsset = reader.IsDBNull(18) ? "" : reader.GetString(18);
            tlAssetPower = reader.IsDBNull(19) ? "" : reader.GetString(19);
            data = reader.IsDBNull(20) ? null : JSONNode.Parse(reader.GetString(20));


            JSONNode jsonJob = reader.IsDBNull(21) ? null : JSONNode.Parse(reader.GetString(21));
            if (jsonJob != null && jsonJob.Count > 0)
            {
                enableJob = new int[jsonJob.Count];
                for (int i = 0; i < jsonJob.Count; i++)
                {
                    string id = jsonJob[i];
                    enableJob[i] = int.Parse(id);
                }
            }
        }
    }
}