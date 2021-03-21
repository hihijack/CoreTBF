using System;
using System.Collections.Generic;
using UnityEngine;

public class RoleGroupDataer : Singleton<RoleGroupDataer>
{
    readonly Dictionary<int, RoleGroupData> _dic = new Dictionary<int, RoleGroupData>();

    public RoleGroupData Get(int id)
    {
        if (_dic.ContainsKey(id))
        {
            return _dic[id];
        }
        else
        {
            var reader = GameData.Inst.ExecuteQueryWithID(GameData.Inst.TABLE_ROLEGROUP, id);
            if (reader.Read())
            {
                var roleGroupData = new RoleGroupData(reader);
                GameData.Inst.EndQuery();
                TryCache(roleGroupData);
                return roleGroupData;
            }
            else
            {
                return null;
            }
        }
    }

    /// <summary>
    /// 随机取出一个
    /// </summary>
    /// <param name="level"></param>
    /// <param name="elite"></param>
    /// <param name="exclude">排除列表</param>
    /// <returns></returns>
    public RoleGroupData GetRandom(int level, bool elite, List<int> exclude)
    {
        var reader = GameData.Inst.ExecuteQuery($"select * from {GameData.Inst.TABLE_ROLEGROUP} where level = {level} and {(elite ? "" : "not")} elite and id not in ({GameUtil.GetStringLst(exclude)}) order by random() limit 1");
        if (reader.Read())
        {
            var roleGroupData = new RoleGroupData(reader);
            GameData.Inst.EndQuery();
            TryCache(roleGroupData);
            return roleGroupData;
        }
        else
        {
            Debug.LogError("没有敌人可以添加了");
        }
        return null;
    }

    private void TryCache(RoleGroupData data)
    {
        if (!_dic.ContainsKey(data.ID))
        {
            _dic.Add(data.ID, data);
        }
    }
}
