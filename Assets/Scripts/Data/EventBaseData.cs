using SimpleJSON;
using System.Data;
using System;

public enum EEventType
{
    Enemy = 1, //敌人
    ChestBox = 2, //宝箱
    Shop = 3, //商人
}

public class EventBaseData 
{
    public int ID;
    public EEventType type;
    public string data;
    public string desc;
    public JSONNode options;

    public EventBaseData(IDataReader reader)
    {
        ID = reader.GetInt16(0);
        type = (EEventType)Enum.Parse(typeof(EEventType), reader.GetString(1));
        data = reader.GetString(2);
        desc = reader.GetString(3);
        options = JSONNode.Parse(reader.GetString(4));
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
            default:
                return "";
        }
    }
}
