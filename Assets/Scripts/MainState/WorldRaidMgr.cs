using System;
using SimpleJSON;
using UI;

public class WorldRaidMgr : Singleton<WorldRaidMgr>
{
    /// <summary>
    /// 世界树战斗事件
    /// </summary>
    /// <param name="eventBaseData"></param>
    /// <param name="data"></param>
    internal void OnEventFight(EventBaseData eventBaseData, JSONNode data)
    {
       GameMgr.Inst.ToState(EGameState.Fight);
    }

    /// <summary>
    /// 事件树显示事件UI
    /// </summary>
    /// <param name="eventBaseData"></param>
    /// <param name="data"></param>
    internal void OnEventShowEventUI(EventBaseData eventBaseData, JSONNode data)
    {
        var ui = UIMgr.Inst.ShowUI(UITable.EUITable.UIWorldEvent) as UIWorldEvent;
        ui.SetData(eventBaseData);
        ui.Refresh();
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

    internal void OnEventLeaveWorld(EventBaseData eventBaseData, JSONNode data)
    {
        GameMgr.Inst.MainState.curInWorld = EWorld.MainWorld;
        UIMgr.Inst.HideAll();
        UIMgr.Inst.ShowUI(UITable.EUITable.UIMainStage);
    }
}