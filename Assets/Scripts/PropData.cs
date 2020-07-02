using UnityEngine;

namespace DefaultNamespace
{
    public class PropData
    {
        public int hp;
        private int _maxHP;
        public int mp;
        public int maxMP;
        public int atk;
        public int def;
        public int shield;
        public int tenacityMax;
        public int tenacity;

        public int defParamAdd;
        public float defParamMul = 1f;

        public int maxHPParamAdd;
        private float _maxHPParamMul = 1f;

        public int Def
        {
            get
            {
                return Mathf.CeilToInt((def + defParamAdd) * defParamMul);
            }
        }

        public int MaxHP 
        { 
            get => Mathf.Max(1, Mathf.CeilToInt((_maxHP + maxHPParamAdd) * MaxHPParamMul)); 
            set => _maxHP = value; 
        }
        public float MaxHPParamMul 
        { 
            get => _maxHPParamMul;
            set 
            {
                _maxHPParamMul = value;
                _maxHPParamMul = Mathf.Max(0, _maxHPParamMul);
            }
        }

        public void ChangeHP(int hpchange)
        {
            hp += hpchange;
            hp = Mathf.Clamp(hp, 0, MaxHP);
        }

        public void ChangeMP(int mpchange)
        {
            mp += mpchange;
            mp = Mathf.Max(0, mp);
        }

        public void ChangeShield(int shieldChanged)
        {
            shield += shieldChanged;
            shield = Mathf.Max(shield, 0);
        }

        public void ChangeTenacity(int change)
        {
            tenacity += change;
            tenacity = Mathf.Clamp(tenacity, 0, tenacityMax);
        }

        /// <summary>
        /// 恢复至满韧性
        /// </summary>
        public void RecoverTenacity()
        {
            tenacity = tenacityMax;
        }

        /// <summary>
        /// 恢复至百分比韧性
        /// </summary>
        /// <param name="val"></param>
        public void RecoverTenactiyByPercent(float val)
        {
            int t = Mathf.CeilToInt(val * tenacityMax);
            tenacity = t;
        }

        /// <summary>
        /// 韧性百分比
        /// </summary>
        /// <returns></returns>
        public float GetTenacityPercent()
        {
            return (float)tenacity / tenacityMax;
        }
    }
}