using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIPlayerInfo : MonoBehaviour
    {
        public Slider sldHP;
        public Text txtHP;
        public Slider sldMP;
        public Text txtMP;

        private static UIPlayerInfo _inst;
        public static UIPlayerInfo Inst
        {
            get { return _inst; }
            
        }

        private void Awake()
        {
            _inst = this;
        }

        public void Refresh()
        {
            sldHP.value = (float)PlayerRolePropDataMgr.Inst.propData.hp / PlayerRolePropDataMgr.Inst.propData.maxHP;
            txtHP.text = PlayerRolePropDataMgr.Inst.propData.hp + "/" + PlayerRolePropDataMgr.Inst.propData.maxHP;
            sldMP.value = (float) PlayerRolePropDataMgr.Inst.propData.mp / PlayerRolePropDataMgr.Inst.propData.maxMP;
            txtMP.text = PlayerRolePropDataMgr.Inst.propData.mp + "/" + PlayerRolePropDataMgr.Inst.propData.maxMP;
        }
    }
}