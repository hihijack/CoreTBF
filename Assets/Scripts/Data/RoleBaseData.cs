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
        public int jobID;
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

        JobBaseData jobData;

        public RoleBaseData(IDataReader reader)
        {
            ID = reader.GetInt16(0);
            name = reader.GetString(1);
            jobID = reader.GetInt16(2);
            model = reader.GetString(3);
            headicon = reader.GetString(4);
            hp = reader.GetInt16(5);
            atk = reader.GetInt16(6);
            def = reader.GetInt16(7);
            resFire = reader.GetInt16(8);
            resThunder = reader.GetInt16(9);
            resDark = reader.GetInt16(10);
            resMagic = reader.GetInt16(11);
            tenacity = reader.GetInt16(12);
            toughness = reader.GetInt16(13);
            speed = reader.GetFloat(14);
            tenClearStiff = reader.GetFloat(15);
            skills = new int[5];
            skills[0] = reader.GetInt16(16);
            skills[1] = reader.GetInt16(17);
            skills[2] = reader.GetInt16(18);
            skills[3] = reader.GetInt16(19);
            skills[4] = reader.GetInt16(20);

            if (!reader.IsDBNull(21))
            {
                JSONNode aiData = JSONNode.Parse(reader.GetString(21));
                int count = aiData.Count;
                aiSkillIndexs = new int[count];
                for (int i = 0; i < count; i++)
                {
                    aiSkillIndexs[i] = aiData[i].AsInt;
                }
            }
        }

        public JobBaseData JobData 
        { 
            get 
            {
                if (jobData == null)
                {
                    jobData = JobDataer.Inst.Get(jobID);
                }
                return jobData;
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