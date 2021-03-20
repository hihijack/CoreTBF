using SimpleJSON;
using System.Data;
using System;
using System.Collections.Generic;
using System.Text;

public enum EEventType
{
    Enemy = 0, 
    ChestBox = 1, 
    Shop = 2,
    BonFire = 3,//篝火
    Other = 4, //随机事件
    NextArea = 5
}

public class EventBaseData 
{
    public string ID;
    public EEventType type;
    public string desc;
    
    public List<string> lstOptions;

    public List<string> lstChildID;
    
    public JSONNode jsonEvents;

    public bool isRoot;
    public int level;
    public bool enableRepetInArea;
    public bool enableRepsetInWorld;
    public bool preset;//是否为预设

    public EventBaseData(IDataReader reader)
    {
        ID = reader.GetString(0);
        type = (EEventType)Enum.Parse(typeof(EEventType), reader.GetString(1));
        desc = reader.IsDBNull(2) ? "" : reader.GetString(2);
        JSONNode jsonOptions = reader.IsDBNull(3) ? null : JSONNode.Parse(reader.GetString(3));
        if (jsonOptions != null && jsonOptions.Count > 0)
        {
            lstOptions = new List<string>();
            for (int i = 0; i < jsonOptions.Count; i++)
            {
                string option = jsonOptions[i];
                lstOptions.Add(option);
            }
        }

        JSONNode jsonChilds = reader.IsDBNull(4) ? null : JSONNode.Parse(reader.GetString(4));
        if (jsonChilds != null && jsonChilds.Count > 0)
        {
            lstChildID = new List<string>();
            for (int i = 0; i < jsonChilds.Count; i++)
            {
                string id = jsonChilds[i];
                lstChildID.Add(id);
            }
        }
       
        string strEnents = reader.IsDBNull(5) ? null : reader.GetString(5);
        if (!string.IsNullOrEmpty(strEnents))
        {
             jsonEvents =JSONNode.Parse(strEnents);
        }
       
        isRoot = reader.GetBoolean(6);
        level = reader.GetInt16(7);
        enableRepetInArea = reader.GetBoolean(8);
        enableRepsetInWorld = reader.GetBoolean(9);
        preset = reader.GetBoolean(10);
    }

    public EventBaseData() { }

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
            case EEventType.Other:
                return "Buffs/Icons8_18";
            default:
                return "";
        }
    }


    public string GetKeyValueStr()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append($"type = '{type.ToString()}',");
        sb.Append($"desc='{desc}',");
        sb.Append($"options='{GetOptionStr()}',");
        sb.Append($"childs='{GetChildStr()}',");
        sb.Append($"events='{GetJsonEventStr()}',");
        sb.Append($"isroot={(isRoot ? 1 : 0)}" + ",");
        sb.Append($"level={level},");
        sb.Append($"enableRepetInArea={(enableRepetInArea ? 1 : 0) },");
        sb.Append($"enableRepetInWorld={(enableRepsetInWorld ? 1 : 0)},");
        sb.Append($"preset={(preset?1:0)}" );
        return sb.ToString();
    }

    public string GetValuesStr()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append($"'{ID}',");
        sb.Append($"'{type.ToString() }',");
        sb.Append($"'{desc}',");
        sb.Append($"'{GetOptionStr() }',");
        sb.Append($"'{GetChildStr()}',");
        sb.Append($"'{GetJsonEventStr()}',");
        sb.Append((isRoot ? 1 : 0)  + ",");
        sb.Append(level + ",");
        sb.Append((enableRepetInArea ? 1 : 0) + ",");
        sb.Append((enableRepsetInWorld ? 1 : 0) + ",");
        sb.Append((preset ? 1 : 0));
        return sb.ToString();
    }

    private string GetJsonEventStr()
    {
        return jsonEvents != null ? jsonEvents.ToString() : "";
    }

    private string GetChildStr()
    {
        var result = GameUtil.GetStringLst(lstChildID);
        if (!string.IsNullOrEmpty(result))
        {
            return $"[{result}]";
        }
        else
        {
            return "";
        }
        
    }

    internal string GetOptionStr()
    {
        var result = GameUtil.GetStringLst(lstOptions);
        if (!string.IsNullOrEmpty(result))
        {
            return $"[{result}]";
        }
        else
        {
            return "";
        }
    }

    public static EventBaseData Gen() 
    {
        var data = new EventBaseData();
        //生成随机唯一ID
        string guid = Guid.NewGuid().ToString("N").Substring(0,5);
        data.ID = guid;
        return data;
    }

    /// <summary>
    /// 将数据更新到数据库
    /// </summary>
    public void UpdateToDB()
    {

    }
}
