using System;
using System.Collections.Generic;

public class EventOptionDataer : Singleton<EventOptionDataer>
{
    readonly Dictionary<int, EventOptionBaseData> _dic = new Dictionary<int, EventOptionBaseData>();

    public EventOptionBaseData Get(int id)
    {
        if (_dic.ContainsKey(id))
        {
            return _dic[id];
        }
        else
        {
            var reader = GameData.Inst.ExecuteQueryWithID(GameData.Inst.TABLE_EVENTOPTIONS, id);
            if (reader.Read())
            {
                var eventOptionData = new EventOptionBaseData(reader);
                GameData.Inst.EndQuery();
                _dic.Add(id, eventOptionData);
                return eventOptionData;
            }
            else
            {
                return null;
            }
        }
    }
}
