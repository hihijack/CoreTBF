using System;
using System.Collections.Generic;

/// <summary>
/// 通过ID筛选
/// </summary>
public class EventFilterInID : EventFilter
{
    string id;
    public EventFilterInID(int odds, string id) : base(odds)
    {
        this.id = id;
    }

    public override EventBaseData GetData()
    {
        return EventDataer.Inst.Get(id);
    }
}
