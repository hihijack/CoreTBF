using System;
using System.Collections.Generic;

public class WorldTreeData : Singleton<WorldTreeData>
{
    public List<WorldTreeNode> lstNodes = new List<WorldTreeNode>();

    public bool HasData()
    {
        return lstNodes.Count > 0;
    }

    public void ClearData()
    {
        lstNodes.Clear();
    }

    public void CreateData(int maxLayer)
    {
        lstNodes.Clear();

        int countLastLaer = 0;

        for (int layer = 0; layer < maxLayer; layer++)
        {
            int countPerLayer = 0;
            if (layer == 0)
            {
                countPerLayer = 3;
            }
            else if (layer == maxLayer - 1)
            {
                countPerLayer = 1;
            }
            else
            {
                countPerLayer = UnityEngine.Random.Range(1, 5);
            }

            int pointT = 0;

            for (int index = 0; index < countPerLayer; index++)
            {
                WorldTreeNode node = new WorldTreeNode();
                node.layer = layer;
                node.index = index;
                node.eventBaseData = GetAEventBaseData();
                node.maxIndexCurLayer = countPerLayer;

                if (countLastLaer > 0)
                {
                    int pStart = pointT;
                    int pEnd = UnityEngine.Random.Range(pointT, countLastLaer);
                    if (index == countPerLayer - 1)
                    {
                        //最后一个节点
                        pEnd = countLastLaer - 1;
                    }
                    for (int p = pStart; p <= pEnd; p++)
                    {
                        node.AddChild(GetNode(layer - 1, p));
                    }
                    pointT = pEnd;
                }

                lstNodes.Add(node);
            }

            countLastLaer =  countPerLayer;
        }
    }

    private WorldTreeNode GetNode(int layer, int index)
    {
        foreach (var item in lstNodes)
        {
            if (item.layer == layer && item.index == index)
            {
                return item;
            }
        }
        return null;
    }

    EventBaseData GetAEventBaseData()
    {
        return EventDataer.Inst.Get(1);
    }

}
