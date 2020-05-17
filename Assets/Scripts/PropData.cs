using UnityEngine;

namespace DefaultNamespace
{
    public class PropData
    {
        public int hp;
        public int maxHP;
        public int mp;
        public int maxMP;
        public int atk;
        public int def;
        
        public void ChangeHP(int hpchange)
        {
            hp += hpchange;
            hp = Mathf.Clamp(hp, 0, maxHP);
        }

        public void ChangeMP(int mpchange)
        {
            mp += mpchange;
            mp = Mathf.Clamp(mp, 0, maxMP);
        }
    }
}