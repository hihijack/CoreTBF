using System;
using SimpleJSON;
using System.Data;

/// <summary>
/// 倒计时结束逻辑
/// </summary>
public enum EBuffTimeOutMode
{
    Clear, //清除
    LayerReduce, //层数减少
    LayerAdd //层数增加
}

public class BuffBaseData
{
    public int ID;
    public EBuffTimeOutMode timeOutMode;
    public string name;
    public string desc;
    public string icon;
    public int maxLayer;
    public JSONNode data;

    public BuffBaseData(IDataReader reader)
    {
        ID = reader.GetInt16(0);
        timeOutMode = (EBuffTimeOutMode)Enum.Parse(typeof(EBuffTimeOutMode), reader.GetString(1));
        name = reader.GetString(2);
        desc = reader.IsDBNull(3)? "" : reader.GetString(3);
        icon = reader.GetString(4);
        maxLayer = reader.GetInt16(5);
        data = reader.IsDBNull(6)? null :JSONNode.Parse(reader.GetString(6));
    }
}