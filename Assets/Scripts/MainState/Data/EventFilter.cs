using System;
using System.Collections.Generic;

/// <summary>
/// 事件过滤器。用于筛选事件
/// </summary>
public class EventFilter
{
    int odds;

    public EventFilter(int odds)
    {
        this.odds = odds;
    }
    
    public bool CheckOdds()
    {
        return odds > UnityEngine.Random.Range(1, 100);
    }

    /// <summary>
    /// 应用筛选器获取数据
    /// </summary>
    /// <returns></returns>
    public virtual EventBaseData GetData() 
    {
        return null;
    }
}
