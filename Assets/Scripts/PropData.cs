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

        /// <summary>
        /// 坚韧度:降低削韧;0~100
        /// </summary>
        public int toughness;
        public int toughnessParamAdd;
        public float toughnessParamMul = 1f;

        public int defParamAdd;
        public float defParamMul = 1f;

        public int maxHPParamAdd;
        private float _maxHPParamMul = 1f;

        public float dmgHurtedMul = 1f;//受到的伤害增加;计算防御之前

        public int Def
        {
            get
            {
                return Mathf.CeilToInt((def + defParamAdd) * defParamMul);
            }
        }

        public int Toughness
        {
            get
            {
                return Mathf.Clamp(Mathf.CeilToInt((toughness + toughnessParamAdd) * toughnessParamMul), 0 , 100);
            }
        }

        /// <summary>
        /// 受到的伤害增加百分百;计算防御之前
        /// </summary>
        public float DmgHurtedMul
        {
            get
            {
                return dmgHurtedMul;
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
        /// 设置韧性为指定百分比.至少1点
        /// </summary>
        /// <param name="percent"></param>
        public void SetTenacityPercent(float percent)
        {
            tenacity = Mathf.CeilToInt(tenacityMax * percent); 
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