using System;
using System.Collections.Generic;

public class EventDataer : Singleton<EventDataer>
{
    readonly Dictionary<int, EventBaseData> _dic = new Dictionary<int, EventBaseData>();

    public EventBaseData Get(int id)
    {
        if (_dic.ContainsKey(id))
        {
            return _dic[id];
        }
        else
        {
            var reader = GameData.Inst.ExecuteQueryWithID(GameData.Inst.TABLE_EVENTS, id);
            if (reader.Read())
            {
                var eventData = new EventBaseData(reader);
                GameData.Inst.EndQuery();
                _dic.Add(id, eventData);
                return eventData;
            }
            else
            {
                return null;
            }
        }
    }

    public EventBaseData GetARandomRootEvent()
    {
        var reader =  GameData.Inst.ExecuteQuery($"SELECT * FROM {GameData.Inst.TABLE_EVENTS} where isroot ORDER BY RANDOM() limit 1");
        if (reader.Read())
            {
                var eventData = new EventBaseData(reader);
                GameData.Inst.EndQuery();
                TryCache(eventData);
                return eventData;
            }
            else
            {
                return null;
            }
    }

    private void TryCache(EventBaseData eventData)
    {
       if (!_dic.ContainsKey(eventData.ID))
       {
           _dic.Add(eventData.ID, eventData);
       }
    }
}
