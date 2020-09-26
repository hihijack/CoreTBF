using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DefaultNamespace;
using System.Diagnostics.Tracing;

public class UIItemTarget : MonoBehaviour
{
    public Image headIcon;
    public Text txtName;
    public Button btn;

    [HideInInspector]
    public UITargetSelectPanel selectPanel;

    public Character target;

    private void Start()
    {
        btn.onClick.AddListener(BtnClick);
    }

    public void Refresh()
    {
        if (target == null)
        {
            return;
        }

        GameUtil.SetSprite(headIcon, target.roleData.headicon);
        txtName.text = target.roleData.name;
    }

    public void BtnClick()
    {
        if (target == null)
        {
            return;
        }
        selectPanel.OnTargetCick(this);
    }
}
