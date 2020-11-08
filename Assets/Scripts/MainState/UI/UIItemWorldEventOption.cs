using System;
using UnityEngine.UI;
using UnityEngine;

public class UIItemWorldEventOption : UIItemBase
{
    public Text txtDesc;
    public int index{private set; get;}
    string desc;
    Action<UIItemWorldEventOption> onSelectOption;

    public override void OnAwake()
    {
        base.OnAwake();
        var btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        onSelectOption(this);
    }

    internal void SetData(int index, string strOption, Action<UIItemWorldEventOption> onSelectOption)
    {
       this.index = index;
       this.desc = strOption;
       this.onSelectOption = onSelectOption;
    }

    public override void Refresh()
    {
        base.Refresh();
        txtDesc.text = desc;
    }
}