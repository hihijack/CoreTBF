using System;
using System.Collections.Generic;
using SimpleJSON;

public class EventProcessor : Singleton<EventProcessor>
{
    public const string EVENT_SHOW = "show";
    public const string EVENT_FIGHT = "fight";
    public const string EVENT_LEAVE_WORLD = "leaveworld";

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
        UnityEngine.Debug.Log("FireEvent:" + key);//########
        if (_dicActions.ContainsKey(key))
        {
            _dicActions[key](eventBaseData,data);
        }
    }    
}