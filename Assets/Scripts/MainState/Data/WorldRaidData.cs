using System.Collections.Generic;

public class WorldRaidData : Singleton<WorldRaidData>
{
    public GraphAdjList<WorldGraphNode> graph;

    public List<CharacterForRaid> lstCharacters;

    public int numOfFood;
    public int numOfGold;
    public int layer;
    public int maxLayer;

    public int curPointIndex;//当前所在位置索引

    public bool HasData()
    {
        return  graph != null;
    }

    public void InitCharacterData(int[] roleIds)
    {
        lstCharacters = new List<CharacterForRaid>();
        foreach (var roleID in roleIds)
        {
            lstCharacters.Add(new CharacterForRaid(roleID));
        }
    }

    /// <summary>
    /// 生成事件树处理器
    /// </summary>
    /// <returns></returns>
    EventTreeHandler GenEventTreeHandler(EventBaseData rootEvent)
    {
        EventTreeHandler eventTreeHalder = new EventTreeHandler();
        eventTreeHalder.GenEventTree(rootEvent);
        return eventTreeHalder;
    }

    public void CreateWorldGraphData(int nodeCount)
    {
        Node<WorldGraphNode>[] nodes = new Node<WorldGraphNode>[nodeCount];
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i] = new Node<WorldGraphNode>(new WorldGraphNode(GenEventTreeHandler(EventDataer.Inst.GetARandomRootEvent())));
        }
        graph = new GraphAdjList<WorldGraphNode>(nodes);
        
        int indexEnd = nodes.Length - 1;

        for (int i = 0; i < nodes.Length; i++)
        {
            int ranEndVal = indexEnd;
            if (i == 0)
            {
                ranEndVal = indexEnd - 1;
            }

            int count = UnityEngine.Random.Range(1, ranEndVal - i + 1);
            count = UnityEngine.Mathf.Min(count, 2);

            var nexts = GameUtil.GetRandomNumbersSet(i + 1, ranEndVal, count);
            if (nexts != null)
            {
                for (int j = 0; j < nexts.Count; j++)
                {
                    foreach (var next in nexts)
                    {
                        graph.SetEdge(i, next, 1); 
                    }
                }  
            }
        }
    }

    public EventBaseData GetEventBaseData()
    {
        return EventDataer.Inst.Get(1);
    }

    internal void Init(int[] roles, int numOfFood)
    {
        InitCharacterData(roles);
        this.numOfFood = numOfFood;
        this.numOfGold = 0;
        this.layer = 1;
        this.maxLayer = 10;
        curPointIndex = -1;
        CreateANewLayerMap();
    }

    /// <summary>
    // 当前所在的节点
    /// </summary>
    /// <returns></returns>
    public WorldGraphNode GetCurInTreeNode()
    {
        if (curPointIndex >= 0)
        {   
            return graph[curPointIndex].Data.Data;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 取可到达点
    /// </summary>
    /// <returns></returns>
    public List<WorldGraphNode> GetEnableTreeNodes()
    {
        List<WorldGraphNode> lst = new List<WorldGraphNode>();
         if (curPointIndex >= 0)
        {   
            var curInVexNode = graph[curPointIndex];
            var p = curInVexNode.FirstAdj;
            while (p != null)
            {
                lst.Add(graph[p.Adjvex].Data.Data);
                p = p.Next;
            }
        }
        else
        {
            //第一个点
           lst.Add(graph[0].Data.Data);
        }
        return lst;
    }

   /// <summary>
    /// 取可到达点索引
    /// </summary>
    /// <returns></returns>
    public List<int> GetEnableTreeNodeIndexs()
    {
        List<int> lst = new List<int>();
         if (curPointIndex >= 0)
        {   
            var curInVexNode = graph[curPointIndex];
            var p = curInVexNode.FirstAdj;
            while (p != null)
            {
                lst.Add(p.Adjvex);
                p = p.Next;
            }
        }
        else
        {
            //第一个点
           lst.Add(0);
        }
        return lst;
    }

    public void CreateANewLayerMap()
    {
        CreateWorldGraphData(UnityEngine.Random.Range(3,8));
    }
}