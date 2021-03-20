using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public struct PlayerItem
{
    public int id;
    public int count;
}


/// <summary>
/// 玩家数据
/// </summary>
[System.Serializable]
public class PlayerData
{
    public List<int> LstCharacterIDUnlocked { get; private set; }

    private List<int> lstSkillIDUnlocked;

    public Dictionary<int, PlayerItem> dicItems;

    public int curLayerInRaid = 1;

    public PlayerData()
    {
        LstCharacterIDUnlocked = new List<int>();
        lstSkillIDUnlocked = new List<int>();
        dicItems = new Dictionary<int, PlayerItem>();
    }

    public List<int> GetLstSkillIDUnlocked()
    {
        return lstSkillIDUnlocked;
    }

    public void AddSkillUnlocked(int skillID)
    {
        if (!lstSkillIDUnlocked.Contains(skillID))
        {
            lstSkillIDUnlocked.Add(skillID);
        }
    }



    /// <summary>
    /// 设置默认初始数据
    /// </summary>
    internal void SetDefault()
    {
        //默认角色
        LstCharacterIDUnlocked.Add(1);
        //默认技能解锁
        lstSkillIDUnlocked.Add(3);
        lstSkillIDUnlocked.Add(4);
        lstSkillIDUnlocked.Add(5);
    }

    internal int GetItemCount(int id)
    {
        if (dicItems.ContainsKey(id))
        {
            return dicItems[id].count;
        }else
        {
            return 0;
        }
    }

    /// <summary>
    /// 设置角色解锁
    /// </summary>
    /// <param name="id"></param>
    public void SetCharacterUnlock(int id)
    {
        if (!LstCharacterIDUnlocked.Contains(id))
        {
            LstCharacterIDUnlocked.Add(id);
        }
    }

    public void ChangeItem(int id, int count)
    {
        if (dicItems.ContainsKey(id))
        {
            var t = dicItems[id];
            t.count += count;
            t.count = Mathf.Max(0, t.count);
            dicItems[id] = t;
        }else
        {
            var t = new PlayerItem();
            t.id = id;
            t.count = count;
            dicItems[id] = t;
        }
    }
}

public class PlayerDataMgr : Singleton<PlayerDataMgr>
{
    const string FILE_RECORDER = "playerdata.dat";
    public PlayerData PlayerData { get; private set; }

    /// <summary>
    /// 保存到存档
    /// </summary>
    public void Save()
    {
        if (PlayerData == null)
        {
            Debug.LogError("playerdata is null");
            return;
        }
        string fileName = Path.Combine(Application.persistentDataPath, FILE_RECORDER);
        Stream fStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
        BinaryFormatter binFormat = new BinaryFormatter();//创建二进制序列化器
        binFormat.Serialize(fStream, PlayerData);
        fStream.Close();
    }

    /// <summary>
    /// 读取存档
    /// </summary>
    public void ReadFromSaved()
    {
        string fileName = Path.Combine(Application.persistentDataPath, FILE_RECORDER);
        if (File.Exists(fileName))
        {
            Stream fStream = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite);
            BinaryFormatter binFormat = new BinaryFormatter();//创建二进制序列化器
            PlayerData = (PlayerData)binFormat.Deserialize(fStream);
            fStream.Close();
            Debug.Log("读取存档完毕");
        }
        else
        {
            PlayerData = new PlayerData();
            PlayerData.SetDefault();
            Debug.Log("创建新存档");
        }
    }
}
