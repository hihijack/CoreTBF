using System;
using System.Collections.Generic;

public class WorldTreeNode
{
    public int layer;
    public int index;
    public int maxIndexCurLayer;
    public EventBaseData eventBaseData;
    public List<WorldTreeNode> childs = new List<WorldTreeNode>();

    internal void AddChild(WorldTreeNode node)
    {
        childs.Add(node);
    }
}
