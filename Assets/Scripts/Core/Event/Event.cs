using System;
using System.Collections.Generic;

public class Event : Singleton<Event>
{
    public enum EEvent
    {
        FIGHT_WIN,
        FIGHT_FAIL,
        ToEventLeaf, //到达事件树叶子节点
        PlayerItemChange, //玩家物品改变
        RAID_LAYER_CHANGE,
        WORLD_NODE_MARK_CLEAR,//节点标记清理
        WORLD_TREE_STATE_UPDATE,
        CHARACTER_DIE,
        CHARACTER_TEAMLOC_CHANGED,
    }

    Dictionary<EEvent, Action<object>> _dic = new Dictionary<EEvent, Action<object>>();

    public void Register(EEvent eventID, Action<object> action)
    {
        if (_dic.ContainsKey(eventID))
        {
            _dic[eventID] += action;
        }else
        {
            _dic.Add(eventID, action);
        }
    }

    public void UnRegister(EEvent eventID, Action<object> action)
    {
        if (_dic.ContainsKey(eventID))
        {
            _dic[eventID] -= action;
        }
    }

    public void Fire(EEvent eventID, object data)
    {
        if (_dic.ContainsKey(eventID))
        {
            _dic[eventID](data);
        }else
        {
            UnityEngine.Debug.LogWarning("Fire Event That Has Not Registered:" + eventID);
        }
    }

    public void Fire(EEvent eventID)
    {
        if (_dic.ContainsKey(eventID))
        {
            _dic[eventID](null);
        }else
        {
            UnityEngine.Debug.LogWarning("Fire Event That Has Not Registered:" + eventID);
        }
    }
}