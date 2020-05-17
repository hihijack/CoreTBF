using Data;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameData : MonoBehaviour
    {
        
        public RoleDataTable roleData;
        public SkillDataTable skillData;
        
        private static GameData _inst;
        public static GameData Inst
        {
            get
            {
                return _inst;
            }
        }


        private void Awake()
        {
            _inst = this;
        }
    }
}