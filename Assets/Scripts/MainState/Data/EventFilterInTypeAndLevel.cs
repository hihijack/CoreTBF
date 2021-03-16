using System;
using System.Collections.Generic;

public class EventFilterInTypeAndLevel : EventFilter
{
    string type;
    int level;

    public EventFilterInTypeAndLevel(int odds, string type, int level) : base(odds)
    {
        this.type = type;
        this.level = level;
    }

    public override EventBaseData GetData()
    {
        //根据type和level随机挑选一个
        //满足不重复条件
        // select * from event where 
        //type = type and level = level and isroot 
        //and (repetInArea or id not in(lstEventsInArea) 
        //and (repetInWorld or id not in(lstEventsInWorld)))
        var reader = GameData.Inst.ExecuteQuery($"select * from {GameData.Inst.TABLE_EVENTS} where type = '{this.type}' and level = {this.level} and isroot and (enableRepetInArea or id not in({GameUtil.GetStringLst(WorldRaidData.Inst.GetEventLstVisitedInArea(), "'{0}'")}) and (enableRepetInWorld or id not in({GameUtil.GetStringLst(WorldRaidData.Inst.GetEventLstVisitedInWorld(), "'{0}'")}))) ORDER BY RANDOM() limit 1");
        if (reader.Read())
        {
            return new EventBaseData(reader);
        }
        else
        {
            return null;
        }
    }
}
