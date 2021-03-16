using System;
using System.Collections.Generic;
using System.Data;

public class JobBaseData
{
    public int ID;
    public string name;
    public int defRoleID;

    public JobBaseData(IDataReader reader)
    {
        ID = reader.GetInt16(0);
        name = reader.GetString(1);
        defRoleID = reader.GetInt16(2);
    }
}
