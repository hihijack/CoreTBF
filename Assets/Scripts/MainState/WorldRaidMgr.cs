using System;
using System.Collections.Generic;
using SimpleJSON;
using UI;
using UnityEngine;

public class WorldRaidMgr : Singleton<WorldRaidMgr>
{
    bool hasInit = false;

    public void Init()
    {
        if (!hasInit)
        {
            EventProcessor.Inst.RegistorEvent(EventProcessor.EVENT_SHOW, OnEventShowEventUI);
            EventProcessor.Inst.RegistorEvent(EventProcessor.EVENT_FIGHT, OnEventFight);
            EventProcessor.Inst.RegistorEvent(EventProcessor.EVENT_LEAVE_WORLD, OnEventLeaveWorld);
            EventProcessor.Inst.RegistorEvent(EventProcessor.EVENT_SHOW_ITEM_GET,OnEventShowItemGet);
            EventProcessor.Inst.RegistorEvent(EventProcessor.EVENT_TO_NEXT_AREA, OnEventToNextArea);
            EventProcessor.Inst.RegistorEvent(EventProcessor.EVENT_MARK_CLEAR, OnEventMarkAsClear);
        }
        hasInit = true;
    }


    /// <summary>
    /// 当选择一个选项
    /// </summary>
    /// <param name="target"></param>
    internal void OnSelectOption(UIItemWorldEventOption target)
    {
       var curNode = WorldRaidData.Inst.GetCurInTreeNode();
       curNode.eventTreeHandler.TriSelection(target.index);
    }

#region Event
    /// <summary>
    /// 标记节点为已清理
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    private void OnEventMarkAsClear(EventBaseData eventBaseData, JSONNode data)
    {
        var curNode = WorldRaidData.Inst.GetCurInTreeNode();
        curNode.SetClearFlag(true);
        Event.Inst.Fire(Event.EEvent.WORLD_NODE_MARK_CLEAR, null);
    }

    /// <summary>
    /// 前往下个区域
    /// </summary>
    /// <param name="eventBaseData"></param>
    /// <param name="data"></param>
    private void OnEventToNextArea(EventBaseData eventBaseData, JSONNode data)
    {
        if (WorldRaidData.Inst.layer < WorldRaidData.Inst.maxLayer)
        {
            WorldRaidData.Inst.ResetOnIntoAArea();
            WorldRaidData.Inst.layer++;
            WorldRaidData.Inst.GenANewAreaData();
            UIWorldTree.Inst.Refresh();
            Event.Inst.Fire(Event.EEvent.RAID_LAYER_CHANGE, null);
        }
    }

    private void OnEventLeaveWorld(EventBaseData eventBaseData, JSONNode data)
    {
        GameMgr.Inst.MainState.curInWorld = EWorld.MainWorld;
        UIMgr.Inst.HideAll();
        UIMgr.Inst.ShowUI(UITable.EUITable.UIMainStage);
    }

    private void OnEventFight(EventBaseData eventBaseData, JSONNode data)
    {
        WorldRaidData.Inst.ClearEnemy();
        JSONNode dataEnemys = data["enemys"];
        int count = dataEnemys.Count;
        for (int i = 0; i < count; i++)
        {
            JSONNode dataEnemy = dataEnemys[i];
            if (dataEnemy["id"] != null)
            {
                WorldRaidData.Inst.AddEnemy(dataEnemy["id"].AsInt);
            }
            else if (dataEnemy["group"] != null)
            {
                JSONNode dataGroup = GameData.Inst.GetSimpleData(GameData.Inst.TABLE_ROLEGROUP, dataEnemy["group"].AsInt);
                int countEnemy = dataGroup.Count;
                for (int j = 0; j < countEnemy; j++)
                {
                    int enemyID = dataGroup[j].AsInt;
                    WorldRaidData.Inst.AddEnemy(enemyID);
                }
            }
            else if (dataEnemy["rangroup"] != null)
            {
                JSONNode dataRanGroup = dataEnemy["rangroup"];
                int ranGroupID = dataRanGroup[UnityEngine.Random.Range(0, dataRanGroup.Count)].AsInt;
                JSONNode dataGroup = GameData.Inst.GetSimpleData(GameData.Inst.TABLE_ROLEGROUP, ranGroupID);
                int countEnemy = dataGroup.Count;
                for (int j = 0; j < countEnemy; j++)
                {
                    int enemyID = dataGroup[j].AsInt;
                    WorldRaidData.Inst.AddEnemy(enemyID);
                }
            }

        }
        GameMgr.Inst.ToState(EGameState.Fight);
    }

    private void OnEventShowEventUI(EventBaseData eventBaseData, JSONNode data)
    {
        var ui = UIMgr.Inst.ShowUI(UITable.EUITable.UIWorldEvent) as UIWorldEvent;
        ui.SetData(eventBaseData);
        ui.Refresh();
    }

    private void OnEventShowItemGet(EventBaseData eventBaseData, JSONNode data)
    {
        JSONNode itemsData = data["items"];
        List<ItemData> lstItems = new List<ItemData>();
        for (int i = 0; i < itemsData.Count; i++)
        {
            JSONNode itemData = itemsData[i];
            int id = itemData["id"].AsInt;
            int count = itemData["count"].AsInt;
            ItemBaseData itemBaseData = ItemDataer.Inst.Get(id);
            ItemData item = new ItemData();
            item.baseData = itemBaseData;
            item.count = count;
            lstItems.Add(item);
            //数据获得物品
            PlayerDataMgr.Inst.PlayerData.ChangeItem(id, count);
        }
        Event.Inst.Fire(Event.EEvent.PlayerItemChange, null);
        var uiGetItem = UIMgr.Inst.ShowUI(UITable.EUITable.UIGetItem) as UIGetItem;
        uiGetItem.SetData(lstItems);
        uiGetItem.Refresh();
    }
#endregion
}