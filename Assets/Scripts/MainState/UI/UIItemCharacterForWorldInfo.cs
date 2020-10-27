using UnityEngine;
using UnityEngine.UI;

public class UIItemCharacterForWorldInfo : UIItemBase
{
    public Slider sldHP;
    public Text txtHP;
    public Image headIcon;

    public Color colorDying;
    public Color colorNormal;

    public CharacterForRaid data;

    public void Set(CharacterForRaid data)
    {
        this.data = data;
    }

    public override void Refresh()
    {
        if (data != null)
        {
            GameUtil.SetSprite(headIcon, data.roleData.headicon);
            sldHP.value = (float)data.propData.hp / data.propData.MaxHP;
            txtHP.text = data.propData.hp + "/" + data.propData.MaxHP;
            if (data.state == ECharacterForRaidState.Dying)
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