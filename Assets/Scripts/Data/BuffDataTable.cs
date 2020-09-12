using UnityEngine;
using System;
using Sirenix.OdinInspector;

[CreateAssetMenu]
public class BuffDataTable : ScriptableObject
{
    [TableList]
    public BuffData[] data;
    
    public BuffData Get(int id)
    {
        foreach (var t in data)
        {
            if (t.ID == id)
            {
                return t;
            }
        }

        return null;
    }
}

public enum EBuffLogic
{
    /// <summary>
    /// 改变防御
    /// </summary>
    ChangeDef, 
    Break
}

[Serializable]
public class BuffData
{
    public int ID;
    public EBuffLogic logic;
    public string name;
    public string desc;
    public string icon;
    public int maxLayer;

    /// <summary>
    /// 持续时间.0表示常驻
    /// </summary>
    [LabelText("持续时间")]
    public float dur;
    public float[] arrParam;
}