using SimpleJSON;
using System.Data;
using System;

public enum EEventType
{
    Enemy = 0, 
    ChestBox = 1, 
    Shop = 2,
    NextArea = 5
}

public class EventBaseData 
{
    public int ID;
    public EEventType type;
    public string desc;
    public JSONNode jsonOptions;
    public JSONNode jsonChilds;
    public JSONNode jsonEvents;
    public bool isRoot;
    public int level;

    public EventBaseData(IDataReader reader)
    {
        ID = reader.GetInt16(0);
        type = (EEventType)Enum.Parse(typeof(EEventType), reader.GetString(1));
        desc = reader.IsDBNull(2) ? "" : reader.GetString(2);
        jsonOptions = reader.IsDBNull(3) ? null : JSONNode.Parse(reader.GetString(3));
        jsonChilds = reader.IsDBNull(4) ? null : JSONNode.Parse(reader.GetString(4));
        string strEnents = reader.IsDBNull(5) ? null : reader.GetString(5);
        if (!string.IsNullOrEmpty(strEnents))
        {
             jsonEvents =JSONNode.Parse(strEnents);
        }
       
        isRoot = reader.GetBoolean(6);
        level = reader.GetInt16(7);
    }

    public string GetIcon()
    {
        switch (type)
        {
            case EEventType.Enemy:
                return "Buffs/Icon1_22";
            case EEventType.ChestBox:
                return "Buffs/Icon4_63";
            case EEventType.Shop:
                return "Buffs/Icon4_63";
            case EEventType.NextArea:
                return "Buffs/Icon2_58";
            default:
                return "";
        }
    }
}
