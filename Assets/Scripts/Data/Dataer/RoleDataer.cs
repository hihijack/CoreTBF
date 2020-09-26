using UnityEngine;
using UnityEditor;
using Data;
using System.Data;
using DefaultNamespace;
using UnityEngine.Experimental.AI;
using System.Collections.Generic;

public class RoleDataer : Singleton<RoleDataer>
{
    readonly Dictionary<int, RoleBaseData> _dic = new Dictionary<int, RoleBaseData>();

   public RoleBaseData Get(int id)
    {
        if (_dic.ContainsKey(id))
        {
            return _dic[id];
        }
        else
        {
            var reader = GameData.Inst.ExecuteQueryWithID(GameData.Inst.TABLE_ROLES, id);
            if (reader.Read())
            {
                var roleData = new RoleBaseData(reader);
                GameData.Inst.EndQuery();
                _dic.Add(id, roleData);
                return roleData;
            }
            else
            {
                return null;
            }
        }
    }
}