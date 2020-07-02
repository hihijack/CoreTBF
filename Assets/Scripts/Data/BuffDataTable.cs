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
    public float dur;
    public float[] arrParam;
}