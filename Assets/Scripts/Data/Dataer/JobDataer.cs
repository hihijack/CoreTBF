using System;
using System.Collections.Generic;

public class JobDataer : Singleton<JobDataer>
{
    readonly Dictionary<int, JobBaseData> _dic = new Dictionary<int, JobBaseData>();

    public JobBaseData Get(int id)
    {
        if (_dic.ContainsKey(id))
        {
            return _dic[id];
        }
        else
        {
            var reader = GameData.Inst.ExecuteQueryWithID(GameData.Inst.TABLE_JOBS, id);
            if (reader.Read())
            {
                var jobData = new JobBaseData(reader);
                GameData.Inst.EndQuery();
                _dic.Add(id, jobData);
                return jobData;
            }
            else
            {
                return null;
            }
        }
    }
}
