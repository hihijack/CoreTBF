using System.Data;
using System.Security.Cryptography;

public class EventOptionBaseData
{
    public int ID;
    public string desc;
    public string data;

    public EventOptionBaseData(IDataReader reader)
    {
        ID = reader.GetInt16(0);
        desc = reader.GetString(1);
        data = reader.GetString(2);
    }
}
