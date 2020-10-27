using System.Collections.Generic;

public class BuffDataer : Singleton<BuffDataer>
{
    readonly Dictionary<int, BuffBaseData> _dic = new Dictionary<int, BuffBaseData>();

    public BuffBaseData Get(int id)
    {
        if (_dic.ContainsKey(id))
        {
            return _dic[id];
        }
        else
        {
            var reader = GameData.Inst.ExecuteQueryWithID(GameData.Inst.TABLE_BUFFS, id);
            if (reader.Read())
            {
                var buffData = new BuffBaseData(reader);
                GameData.Inst.EndQuery();
                _dic.Add(id, buffData);
                return buffData;
            }
            else
            {
                return null;
            }
        }
    }
    
}