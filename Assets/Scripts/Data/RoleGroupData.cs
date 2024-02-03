using SimpleJSON;
using System.Data;

public class RoleGroupData
{
    public int ID;
    public int[] roles;
    public int level;
    public bool elite;

    public RoleGroupData(IDataReader reader) 
    {
        ID = reader.GetInt16(0);
        var data = JSONNode.Parse(reader.GetString(1));
        roles = new int[data.Count];
        for (int i = 0; i < data.Count; i++)
        {
            roles[i] = data[i].AsInt;
        }
        level = reader.GetInt16(2);
        elite = reader.GetBoolean(3);
    }
}
