using System;
using System.Collections.Generic;
using SimpleJSON;

public class EventProcessor : Singleton<EventProcessor>
{
    public const string EVENT_SHOW = "show";
    public const string EVENT_FIGHT = "fight";
    public const string EVENT_LEAVE_WORLD = "leaveworld";
    public const string EVENT_SHOW_ITEM_GET = "show_item_get";
    public const string EVENT_TO_NEXT_AREA = "to_next_area";
    /// <summary>
    /// 标记节点为已清理
    /// </summary>
    public const string EVENT_MARK_CLEAR = "mark_clear";

    /// <summary>
    /// 随机
    /// </summary>
    public const string EVENT_RANDOM = "random";

    Dictionary<string, Action<EventBaseData,JSONNode>> _dicActions;

    public void Init()
    {
        _dicActions = new Dictionary<string, Action<EventBaseData,JSONNode>>();
    }

    public void RegistorEvent(string key, Action<EventBaseData,JSONNode> action)
    {
        if (!_dicActions.ContainsKey(key))
        {   
            _dicActions.Add(key, action);
        }
    }

    public void UnRegistorEvent(string key)
    {
         if (_dicActions.ContainsKey(key))
        {   
            _dicActions.Remove(key);
        }
    }

    public void Clear()
    {
        _dicActions.Clear();
    }

    public void FireEvent(string key, EventBaseData eventBaseData, JSONNode data)
    {
        UnityEngine.Debug.Log("FireEvent:" + key);//###############
        if (_dicActions.ContainsKey(key))
        {
            _dicActions[key](eventBaseData,data);
        }
    }    
}