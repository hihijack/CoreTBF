using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class UIWorldInfo : UIBase
{
    public Transform tfGridChraracters;
    public GameObject pfbUIItemChracter;
    public Text txtNumOfFood;
    public Text txtNumOfGold;
    public Text txtLayer;

    List<UIItemCharacterForWorldInfo> lstUIItemCharacters;

    public override void Init()
    {
        base.Init();
        lstUIItemCharacters = new List<UIItemCharacterForWorldInfo>();
        Event.Inst.Register(Event.EEvent.PlayerItemChange, OnEventPlayerItemChange);
        Event.Inst.Register(Event.EEvent.RAID_LAYER_CHANGE, OnEventLayerChange);
    }

    private void OnEventLayerChange(object obj)
    {
        RefreshLayer();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        Event.Inst.UnRegister(Event.EEvent.PlayerItemChange, OnEventPlayerItemChange);
        Event.Inst.UnRegister(Event.EEvent.RAID_LAYER_CHANGE, OnEventLayerChange);
    }

    private void OnEventPlayerItemChange(object obj)
    {
        RefreshItem();
    }

    void RefreshItem()
    {
        var numOfFood = PlayerDataMgr.Inst.PlayerData.GetItemCount(GameCfg.ID_FOOD);
        var numOfGold = PlayerDataMgr.Inst.PlayerData.GetItemCount(GameCfg.ID_GOLD);
        txtNumOfFood.text = "x" + numOfFood;
        txtNumOfGold.text = "x" + numOfGold;
    }

    public override void OnShow()
    {
        base.OnShow();
        //角色列表
        for (int i = 0; i < WorldRaidData.Inst.lstCharacters.Count; i++)
        {
            var data = WorldRaidData.Inst.lstCharacters[i];
            var uiItem = UIItemBase.Create<UIItemCharacterForWorldInfo>(tfGridChraracters.transform, pfbUIItemChracter);
            uiItem.Set(data);
            uiItem.Refresh();
            lstUIItemCharacters.Add(uiItem);
        }
        //食物
        RefreshItem();
        RefreshLayer();
    }

    void RefreshLayer()
    {
        txtLayer.text = $"{WorldRaidData.Inst.layer}/{WorldRaidData.Inst.maxLayer}层";
    }

    public override void OnHide()
    {
        base.OnHide();
        foreach (var item in lstUIItemCharacters)
        {
            item.Cache();
        }
    }
}
