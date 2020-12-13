using DefaultNamespace;
using SimpleJSON;
using Sirenix.OdinInspector;
using System.Data;
using System.Text;
using UnityEngine;

namespace Data
{

    public class RoleBaseData
    {
        public int ID;
        public string name;
        public string model;
        public string headicon;
        public int hp;
        public int[] skills;
        public int atk;
        public int def;
        public int resFire;
        public int resThunder;
        public int resDark;
        public int resMagic;
        public float speed;
        public int tenacity;
        /// <summary>
        /// 坚韧度
        /// </summary>
        public int toughness;
        /// <summary>
        /// 韧性清空硬直时间
        /// </summary>
        public float tenClearStiff;

        public int[] aiSkillIndexs;

        public RoleBaseData(IDataReader reader)
        {
            ID = reader.GetInt16(0);
            name = reader.GetString(1);
            model = reader.GetString(2);
            headicon = reader.GetString(3);
            hp = reader.GetInt16(4);
            atk = reader.GetInt16(5);
            def = reader.GetInt16(6);
            resFire = reader.GetInt16(7);
            resThunder = reader.GetInt16(8);
            resDark = reader.GetInt16(9);
            resMagic = reader.GetInt16(10);
            tenacity = reader.GetInt16(11);
            toughness = reader.GetInt16(12);
            speed = reader.GetFloat(13);
            tenClearStiff = reader.GetFloat(14);
            skills = new int[5];
            skills[0] = reader.GetInt16(15);
            skills[1] = reader.GetInt16(16);
            skills[2] = reader.GetInt16(17);
            skills[3] = reader.GetInt16(18);
            skills[4] = reader.GetInt16(19);

            if (!reader.IsDBNull(20))
            {
                JSONNode aiData = JSONNode.Parse(reader.GetString(20));
                int count = aiData.Count;
                aiSkillIndexs = new int[count];
                for (int i = 0; i < count; i++)
                {
                    aiSkillIndexs[i] = aiData[i].AsInt;
                }
            }
        }

        public string GetPropDesc()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"生命:{hp}");
            sb.AppendLine($"攻击力:{atk}");
            sb.AppendLine($"防御力:{def}");
            sb.AppendLine($"火焰抗性:{resFire}");
            sb.AppendLine($"闪电抗性:{resThunder}");
            sb.AppendLine($"黑暗抗性:{resDark}");
            sb.AppendLine($"魔力抗性:{resMagic}");
            sb.AppendLine($"韧性:{tenacity}");
            sb.AppendLine($"强韧度:{toughness}");
            sb.AppendLine($"速度:{speed}");
            return sb.ToString();
        }
    }
}