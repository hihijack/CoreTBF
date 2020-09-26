using Sirenix.OdinInspector;
using System.Data;
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
            speed = reader.GetFloat(8);
            tenacity = reader.GetInt16(9);
            toughness = reader.GetInt16(10);
            tenClearStiff = reader.GetFloat(11);
            skills = new int[5];
            skills[0] = reader.GetInt16(12);
            skills[1] = reader.GetInt16(13);
            skills[2] = reader.GetInt16(14);
            skills[3] = reader.GetInt16(15);
            skills[4] = reader.GetInt16(16);
        }
    }
}