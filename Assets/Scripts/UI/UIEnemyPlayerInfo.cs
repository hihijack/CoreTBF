using System.Threading;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIEnemyPlayerInfo : MonoBehaviour
    {
        public Slider sldHP;
        public Text txtHP;
        public Slider sldMP;
        public Text txtMP;
        
        private static UIEnemyPlayerInfo _inst;
        public static UIEnemyPlayerInfo Inst
        {
            get { return _inst; }
            
        }

        private void Awake()
        {
            _inst = this;
        }

        private Character target;
        
        public void SetData(Character target)
        {
            this.target = target;
        }
        
        public void Refresh()
        {
            if (target == null)
            {
                return;
            }
            
            sldHP.value = (float)target.propData.hp / target.propData.maxHP;
//            txtHP.text = target.propData.hp + "/" + target.propData.maxHP;
//            sldMP.value = (float) target.propData.mp / target.propData.maxMP;
//            txtMP.text = target.propData.mp + "/" + target.propData.maxHP;
        }
    }
}