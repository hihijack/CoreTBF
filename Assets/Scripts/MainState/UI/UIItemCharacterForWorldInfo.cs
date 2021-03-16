using System;
using UnityEngine;
using UnityEngine.UI;

public class UIItemCharacterForWorldInfo : UIItemBase
{
    public Slider sldHP;
    public Text txtHP;
    public Image headIcon;

    public Button btn;

    public Color colorDying;
    public Color colorNormal;

    public CharacterForRaid data;

    Action<UIItemCharacterForWorldInfo> cbClick;

    public void Set(CharacterForRaid data, Action<UIItemCharacterForWorldInfo> cbClick)
    {
        this.data = data;
        this.cbClick = cbClick;
    }

    public override void OnAwake()
    {
        base.OnAwake();
        btn.onClick.AddListener(OnBtnClick);
    }

    private void OnBtnClick()
    {
        cbClick?.Invoke(this);
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