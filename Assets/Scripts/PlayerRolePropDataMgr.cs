using System.Diagnostics;
using Data;
using UI;

namespace DefaultNamespace
{
    public class PlayerRolePropDataMgr
    {
        #region 单例
        private static PlayerRolePropDataMgr _inst;

        public static PlayerRolePropDataMgr Inst
        {
            get
            {
                if (_inst == null)
                {
                    _inst = new PlayerRolePropDataMgr();
                }

                return _inst;
            }
        }
        #endregion

        public PropData propData;
        
        public void Init()
        {
            int maxHP = 0;
            int maxMP = 0;
            propData = new PropData();
            foreach (var character in GameMgr.Inst.lstCharacters)
            {
                if (character.camp == ECamp.Ally)
                {
                    maxHP += character.roleData.HP;
                    maxMP += character.roleData.MP;
                }
            }

            propData.maxHP = maxHP;
            propData.maxMP = maxMP;
            propData.hp = maxHP;
            propData.mp = maxMP;
        }
        
        public void ChangeHP(int hpchange)
        {
            propData.ChangeHP(hpchange);
            UIPlayerInfo.Inst.Refresh();
        }

        public void ChangeMP(int mpchange)
        {
            propData.ChangeMP(mpchange);
            UIPlayerInfo.Inst.Refresh();
        }
    }
}