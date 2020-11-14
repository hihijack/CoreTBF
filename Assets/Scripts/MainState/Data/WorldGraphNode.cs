public class WorldGraphNode
{   
    public EventTreeHandler eventTreeHandler;

    public bool hasClear;//已清理.不会再触发事件

    public bool arrivable;//可达到状态

    public WorldGraphNode(EventTreeHandler eventTreeHandler)
    {
        this.eventTreeHandler = eventTreeHandler;
        hasClear = false;
        arrivable = false;
    }

    public void SetClearFlag(bool clear)
    {
        hasClear = clear;
    }
}