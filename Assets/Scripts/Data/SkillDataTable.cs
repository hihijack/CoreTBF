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
        Atk
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

        public ESkillLogic logic;

        public ESkillTarget targetType;
        
        [VerticalGroup("伤害"), LabelWidth(60)]
        public float dmg,dmgFire;

        public int targetCount;
        
        public float timePower;
        public float backswing;
        public float timeAtkStiff;
        public int tenacityAtk;
        public int cost;

        /// <summary>
        /// 产生仇恨
        /// </summary>
        public int hatred;

        public bool quick;
        [TextArea]
        public string tip;

        public TimelineAsset tlAsset;
    }
}