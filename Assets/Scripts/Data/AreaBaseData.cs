using SimpleJSON;
using System;
using System.Data;
using UnityEngine;

public enum EEventFilterMode
{
    ALL_APPLY = 1,
    IN_ORDER = 2
}

/// <summary>
/// 区域基础数据
/// </summary>
public class AreaBaseData
{
    public int ID;
    public string name;
    public int level;
    public string desc;
    public int minNodeCount;
    public int maxNodeCount;
    public EEventFilterMode mode;
    public JSONNode jsonFilter;

    EventFilter[] eventFilters;

    public AreaBaseData(IDataReader reader) 
    {
        ID = reader.GetInt16(0);
        name = reader.GetString(1);
        level = reader.GetInt16(2);
        desc = reader.GetString(3);
        minNodeCount = reader.GetInt16(4);
        maxNodeCount = reader.GetInt16(5);
        var strMode = reader.GetString(6);
        if (strMode.Equals("all_apply"))
        {
            mode = EEventFilterMode.ALL_APPLY;
        }
        else if (strMode.Equals("in_order"))
        {
            mode = EEventFilterMode.IN_ORDER;
        }
        jsonFilter = JSONNode.Parse(reader.GetString(7));

        //解析成过滤器
        ParseToFilter(jsonFilter);

    }

    public EventFilter[] GetEventFilters() 
    {
        return this.eventFilters;
    }

    private void ParseToFilter(JSONNode jsonFilter)
    {
        var count = jsonFilter.Count;
        eventFilters = new EventFilter[count];
        for (int i = 0; i < count; i++)
        {
            var jsonNode = jsonFilter[i];
            EventFilter filter = null;
            if (jsonNode["id"] != null)
            {
                filter = new EventFilterInID(jsonNode["odds"].AsInt, (jsonNode["id"]));
            }
            else if (jsonNode["type"] != null && jsonNode["level"] != null)
            {
                filter = new EventFilterInTypeAndLevel(jsonNode["odds"].AsInt, jsonNode["type"].ToString(), jsonNode["level"].AsInt);
            }
            else
            {
                Debug.LogError("不支持的事件过滤类型:" + ID);
            }
            if (filter != null)
            {
                eventFilters[i] = filter;
            }

        }
    }
}
