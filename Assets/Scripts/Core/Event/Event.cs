using System;
using System.Collections.Generic;

public class Event : Singleton<Event>
{
    public enum EEvent
    {
        FIGHT_WIN,
        FIGHT_FAIL
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
}