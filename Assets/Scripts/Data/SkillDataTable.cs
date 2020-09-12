using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Timeline;

namespace Data
{
    [CreateAssetMenu]
    public class SkillDataTable : ScriptableObject
    {
        [TableList]
        public SkillData[] data;

        public SkillData Get(int id)
        {
            foreach (var t in data)
            {
                if (t.ID == id)
                {
                    return t;
                }
            }

            return null;
        }
    }

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
    
    [System.Serializable]
    public class SkillData
    {
        public int ID;
        public string name;

        [LabelText("逻辑")]
        public ESkillLogic logic;

        [LabelText("目标类型")]
        public ESkillTarget targetType;
        
        [VerticalGroup("伤害"), LabelWidth(60)]
        public float dmg,dmgFire;

        [LabelText("添加BUFF")]
        public int[] buffsAdd;

        [LabelText("目标数量")]
        public int targetCount;
        
        [LabelText("蓄力时间")]
        public float timePower;
        [LabelText("后摇")]
        public float backswing;
        [LabelText("目标硬直")]
        public float timeAtkStiff;
        [LabelText("削韧")]
        public int tenacityAtk;
        public int cost;
        [LabelText("能量获得")]
        public int mpGet;

        /// <summary>
        /// 韧性变化至指定百分比
        /// </summary>
        [LabelText("韧性变化")]
        public float tenChangeTo;

        /// <summary>
        /// 开始蓄力时的韧性变化
        /// </summary>
        [LabelText("蓄力韧性变化")]
        public float tenChangeToPower;

        //技能目标优先队伍序号.0表示随机
        [LabelText("目标位置")]
        public int targetTeamLoc;

        [TextArea]
        public string tip;

        public TimelineAsset tlAsset;
        public TimelineAsset tlAssetPower;
    }
}