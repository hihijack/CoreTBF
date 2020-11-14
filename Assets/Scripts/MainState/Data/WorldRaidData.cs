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

    int cachedOption = -1;

    public WorldRaidData()
    {
        Event.Inst.Register(Event.EEvent.FIGHT_WIN, OnFightWin);
        Event.Inst.Register(Event.EEvent.FIGHT_FAIL, OnFightFail);
    }

    private void OnFightFail(object obj)
    {
       cachedOption = 1;
    }

    private void OnFightWin(object obj)
    {
        cachedOption = 0;
    }

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

    internal void SetCurPointIndex(int index)
    {
        var oriIndex = curPointIndex;
        curPointIndex = index;
        if (curPointIndex != oriIndex)
        {
            UpdateNodeArrivableState();
            Event.Inst.Fire(Event.EEvent.WORLD_TREE_STATE_UPDATE);
        }
    }

    internal void TryTriCachedOption()
    {
        if (cachedOption >= 0 && curPointIndex >= 0)
        {
            var curNode = GetCurInTreeNode();
            curNode.eventTreeHandler.TriSelection(cachedOption);
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
        int indexEnd = nodes.Length - 1;

        for (int i = 0; i < nodes.Length; i++)
        {
            if (i == indexEnd)
            {
                //终点
                nodes[i] = new Node<WorldGraphNode>(new WorldGraphNode(GenEventTreeHandler(EventDataer.Inst.Get(GameCfg.ID_NEXT_AREA))));
            }else
            {
                nodes[i] = new Node<WorldGraphNode>(new WorldGraphNode(GenEventTreeHandler(EventDataer.Inst.GetARandomRootEvent())));
            }

            if (i == 0)
            {
                //第一个点是可达的
                nodes[i].Data.arrivable = true;
            }

        }

        graph = new GraphAdjList<WorldGraphNode>(nodes);
        

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

    internal void Init(int[] roles)
    {
        InitCharacterData(roles);
        this.layer = 1;
        this.maxLayer = 10;
        curPointIndex = -1;
        CreateANewLayerMap();
    }

    /// <summary>
    /// 进入一个新区域,重置一些数据
    /// </summary>
    public void ResetOnIntoAArea()
    {
        curPointIndex = -1;
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

    /// <summary>
    /// 更新节点可到达性
    /// </summary>
    void UpdateNodeArrivableState()
    {
        int numOfNode = graph.GetNumOfVertex();
        var lstEnableTreeNodeIndexs = GetEnableTreeNodeIndexs();
        for (int i = 0; i < numOfNode; i++)
        {
            var vertNode = WorldRaidData.Inst.graph[i];
            vertNode.Data.Data.arrivable = false;
        }
        if (curPointIndex >= 0)
        {   
            var curInVexNode = graph[curPointIndex];
            var p = curInVexNode.FirstAdj;
            while (p != null)
            {
                graph[p.Adjvex].Data.Data.arrivable = true;
                p = p.Next;
            }
        }
        else
        {
            //第一个点
            graph[0].Data.Data.arrivable = true;
        }  
    }

    public void CreateANewLayerMap()
    {
        CreateWorldGraphData(UnityEngine.Random.Range(3,8));
    }

    internal void FightWin()
    {
        throw new System.NotImplementedException();
    }

    internal void FightFail()
    {
        throw new System.NotImplementedException();
    }
}