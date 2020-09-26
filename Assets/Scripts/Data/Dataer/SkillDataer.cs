using System.Collections.Generic;
using Data;

public class SkillDataer : Singleton<SkillDataer>
{
    readonly Dictionary<int, SkillBaseData> _dic = new Dictionary<int, SkillBaseData>();

    public SkillBaseData Get(int id)
    {
        if (_dic.ContainsKey(id))
        {
            return _dic[id];
        }
        else
        {
            var reader = GameData.Inst.ExecuteQueryWithID(GameData.Inst.TABLE_SKILLS, id);
            if (reader.Read())
            {
                var skillData = new SkillBaseData(reader);
                GameData.Inst.EndQuery();
                _dic.Add(id, skillData);
                return skillData;
            }
            else
            {
                return null;
            }
        }
    }
}