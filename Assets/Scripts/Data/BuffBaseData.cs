using UnityEngine;
using System;
using Sirenix.OdinInspector;
using SimpleJSON;
using System.Data;

public enum EBuffLogic
{
    /// <summary>
    /// 改变防御
    /// </summary>
    ChangeDef, 
    Break
}

public class BuffBaseData
{
    public int ID;
    public EBuffLogic logic;
    public string name;
    public string desc;
    public string icon;
    public int maxLayer;
    public JSONNode data;

    public BuffBaseData(IDataReader reader)
    {
        ID = reader.GetInt16(0);
        logic = (EBuffLogic)Enum.Parse(typeof(EBuffLogic), reader.GetString(1));
        name = reader.GetString(2);
        desc = reader.GetString(3);
        icon = reader.GetString(4);
        maxLayer = reader.GetInt16(5);
        data = JSONNode.Parse(reader.GetString(6));
    }
}