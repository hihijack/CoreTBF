using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIPlayerInfo : UIItemBase
    {
        public Slider sldHP;
        public Slider sldTen;
        public Text txtHP;
        public Image headIcon;

        public Color colorDying;
        public Color colorNormal;

        public Character data;

        public override void Refresh()
        {
            if (data != null)
            {
                GameUtil.SetSprite(headIcon, data.roleData.headicon);
                sldHP.value = (float)data.propData.hp / data.propData.MaxHP;
                sldTen.value = (float)data.propData.tenacity / data.propData.tenacityMax;
                txtHP.text = data.propData.hp + "/" + data.propData.MaxHP;
                if (data.State == ECharacterState.Dying)
                {
                    sldHP.targetGraphic.color = colorDying;
                }
                else
                {
                    sldHP.targetGraphic.color = colorNormal;
                }
            }
        }
    }
}