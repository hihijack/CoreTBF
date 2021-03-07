using System;
using System.Collections.Generic;
using System.Text;

public class AreaDataer : Singleton<AreaDataer>
{
    readonly Dictionary<int, AreaBaseData> _dic = new Dictionary<int, AreaBaseData>();

    public AreaBaseData Get(int id)
    {
        if (_dic.ContainsKey(id))
        {
            return _dic[id];
        }
        else
        {
            var reader = GameData.Inst.ExecuteQueryWithID(GameData.Inst.TABAL_AREA, id);
            if (reader.Read())
            {
                var areaData = new AreaBaseData(reader);
                GameData.Inst.EndQuery();
                _dic.Add(id, areaData);
                return areaData;
            }
            else
            {
                return null;
            }
        }
    }

    public AreaBaseData GetARandomAreaByLevelAndExclude(int level, List<int> lstExclude) 
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < lstExclude.Count; i++)
        {
            if (i != 0)
            {
                sb.Append(",");
            }
            sb.Append(lstExclude[i]);
        }
        
        var reader = GameData.Inst.ExecuteQuery($"SELECT * FROM {GameData.Inst.TABAL_AREA} where level = {level} and id not in ({sb.ToString()}) ORDER BY RANDOM() limit 1");
        if (reader.Read())
        {
            var areaData = new AreaBaseData(reader);
            GameData.Inst.EndQuery();
            _dic.Add(areaData.ID, areaData);
            return areaData;
        }
        else
        {
            return null;
        }
    }
}
