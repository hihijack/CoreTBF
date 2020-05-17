using Sirenix.OdinInspector;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu]
    public class RoleDataTable : ScriptableObject
    {
        [TableList]
        public RoleData[] data;

        public RoleData Get(int id)
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

    [System.Serializable]
    public class RoleData
    {
        public int ID;
        public string name;
        public string model;
        public string headicon;
        public int HP;
        public int MP;
        public int[] skills;
        public int atk;
        public int def;
        public int resFire;
        public float speed;
        public int tenacity;
    }
}