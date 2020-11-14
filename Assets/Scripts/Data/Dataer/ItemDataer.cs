using System.Collections.Generic;

public class ItemDataer : Singleton<ItemDataer>
{
    readonly Dictionary<int, ItemBaseData> _dic = new Dictionary<int, ItemBaseData>();
     public ItemBaseData Get(int id)
     {
         if (_dic.ContainsKey(id))
        {
            return _dic[id];
        }
        else
        {
            var reader = GameData.Inst.ExecuteQueryWithID(GameData.Inst.TABLE_ITEMS, id);
            if (reader.Read())
            {
                var itemData = new ItemBaseData(reader);
                GameData.Inst.EndQuery();
                _dic.Add(id, itemData);
                return itemData;
            }
            else
            {
                return null;
            }
        }
     }
}