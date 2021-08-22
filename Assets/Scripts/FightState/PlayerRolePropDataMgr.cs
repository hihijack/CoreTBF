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
            propData = new PropData();
            propData.mp = 0;
        }

        public void ChangeMP(int mpchange)
        {
            propData.ChangeMP(mpchange);
        }
    }
}