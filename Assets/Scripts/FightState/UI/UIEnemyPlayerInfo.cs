using System.Threading;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIEnemyPlayerInfo : UIItemBase
    {
        public Image headIcon;
        public Slider sldHP;
        public Slider sldTen;
        public Text txtHP;
        public Slider sldMP;
        public Text txtMP;

        private Character target;
        
        public void SetData(Character target)
        {
            this.target = target;
        }
        
        public override void Refresh()
        {
            if (target == null)
            {
                return;
            }

            GameUtil.SetSprite(headIcon, target.roleData.headicon);

            sldHP.value = (float)target.propData.hp / target.propData.MaxHP;
            sldTen.value = (float)target.propData.tenacity / target.propData.tenacityMax;
        }
    }
}