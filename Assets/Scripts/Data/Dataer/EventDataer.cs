using System;
using System.Collections.Generic;

public class EventDataer : Singleton<EventDataer>
{
    readonly Dictionary<string, EventBaseData> _dic = new Dictionary<string, EventBaseData>();

    public EventBaseData Get(string id)
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
        var reader =  GameData.Inst.ExecuteQuery($"SELECT * FROM {GameData.Inst.TABLE_EVENTS} where isroot and level > 0 ORDER BY RANDOM() limit 1");
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
            UnityEngine.Debug.Log("add event:" + eventData.desc);
           _dic.Add(eventData.ID, eventData);
       }
    }

    public void InitAllEventData() 
    {
        var reader = GameData.Inst.ExecuteQuery($"select * from {GameData.Inst.TABLE_EVENTS}");
        while (reader.Read())
        {
            var eventData = new EventBaseData(reader);
            TryCache(eventData);
        }
        GameData.Inst.EndQuery();
    }

    public Dictionary<string, EventBaseData> GetDic()
    {
        return _dic;
    }

    public EventBaseData NewData()
    {
        return EventBaseData.Gen();
    }

    public void Release() 
    {
        _dic.Clear();
    }

    public bool UpdateToDB(EventBaseData data, bool isNewAdd)
    {
        int result = 0;
        if (isNewAdd)
        {
            result = GameData.Inst.Execute($"insert into {GameData.Inst.TABLE_EVENTS} values({data.GetValuesStr()})");
        }
        else
        {
            result = GameData.Inst.Execute($"update {GameData.Inst.TABLE_EVENTS} set {data.GetKeyValueStr()} where id = '{data.ID}'");
        }
        return result > 0;
    }
}
