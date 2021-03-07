using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using System;

public class UIWorldEvent : UIBase
{
    public Text txtDesc;
    public GameObject gridOptions;
    public GameObject pfbOptions;

    EventBaseData eventBaseData;

    List<UIItemWorldEventOption> lstOptions;

    public override void Init()
    {
        base.Init();
        lstOptions = new List<UIItemWorldEventOption>();
        Event.Inst.Register(Event.EEvent.ToEventLeaf, OnToEventLeaf);
    }

    private void OnToEventLeaf(object data)
    {
        UI.UIMgr.Inst.HideUI(UITable.EUITable.UIWorldEvent);
    }

    public override void OnHide()
    {
        base.OnHide();
        foreach (var item in lstOptions)
        {
            item.Cache();
        }
        lstOptions.Clear();
    }

    public void SetData(EventBaseData data)
    {
        this.eventBaseData = data;
    }

    public void Refresh()
    {
        ClearOptions();
        txtDesc.text = eventBaseData.desc;
        AddOptions();
    }

    private void AddOptions()
    {
        for (int i = 0; i < eventBaseData.lstOptions.Count; i++)
        {
            var strOption = eventBaseData.lstOptions[i];
            var uiItemOption = UIItemBase.Create<UIItemWorldEventOption>(gridOptions.transform, pfbOptions);
            uiItemOption.SetData(i, strOption, OnSelectOption);
            uiItemOption.Refresh();
            lstOptions.Add(uiItemOption);
        }
    }

    private void OnSelectOption(UIItemWorldEventOption target)
    {
        WorldRaidMgr.Inst.OnSelectOption(target);
    }

    private void ClearOptions()
    {
        foreach (var item in lstOptions)
        {
            item.Cache();
        }
        lstOptions.Clear();
    }
}