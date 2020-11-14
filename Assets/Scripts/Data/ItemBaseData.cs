
using System.Data;

public class ItemBaseData
{
    public int ID;
    public string name;

    public ItemBaseData(IDataReader reader)
    {
        ID = reader.GetInt16(0);
        name = reader.GetString(1);
    }
}