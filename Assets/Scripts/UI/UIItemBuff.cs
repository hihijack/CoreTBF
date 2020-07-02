using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIItemBuff : MonoBehaviour
{
    public Image icon;
    public Image mask;
    public Text txtDur;

    public BuffBase data;

    public void SetTarget(BuffBase buff)
    {
        data = buff;
    }

    public void Refresh()
    {
        //icon
        GameUtil.SetSprite(icon, "Sprites/Buffs" , data.GetBuffData().icon);
        //dur
        var durLeft = data.GetDurLeft();
        txtDur.text = durLeft > 0 ? durLeft.ToString("0.0") : "N/A";
        //mask
        var prog = data.GetDurProg();
        mask.fillAmount = prog > 0 ? prog : 0;
    }

    public void OnHoverIn()
    {

    }

    public void OnHoverOut()
    {

    }
}
