using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIPlayerInfo : MonoBehaviour
    {
        public Slider sldHP;
        public Text txtHP;
        public Image headIcon;

        public Color colorDying;
        public Color colorNormal;

        public Character data;

        public void Refresh()
        {
            if (data != null)
            {
                GameUtil.SetSprite(headIcon, data.roleData.headicon);
                sldHP.value = (float)data.propData.hp / data.propData.MaxHP;
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