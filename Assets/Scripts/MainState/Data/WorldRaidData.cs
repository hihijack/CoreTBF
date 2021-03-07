using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldRaidData : Singleton<WorldRaidData>
{
    public GraphAdjList<WorldGraphNode> graph;

    public List<CharacterForRaid> lstCharacters;

    List<int> lstEnemyID = new List<int>();

    public int numOfFood;
    public int numOfGold;
    public int layer;
    public int maxLayer;

    public int curPointIndex;//当前所在位置索引

    int cachedOption = -1;

    public int worldLevel; //世界强度

    List<int> lstAreaVisited = new List<int>(); //到达过的区域

    List<string> lstEventVisitedInWorld = new List<string>();//遭遇过的事件

    List<string> lstEnentVisitedInCurArea = new List<string>();//本区域遭遇过的事件

    AreaBaseData mCurArea;

    public WorldRaidData()
    {
        Event.Inst.Register(Event.EEvent.FIGHT_WIN, OnFightWin);
        Event.Inst.Register(Event.EEvent.FIGHT_FAIL, OnFightFail);
    }

    public void AddEnemy(int id) 
    {
        lstEnemyID.Add(id);
    }

    public List<int> GetEnemyLst() 
    {
        return lstEnemyID;
    }

    public void ClearEnemy() 
    {
        lstEnemyID.Clear();
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

    /// <summary>
    /// 为一个区域生成节点
    /// </summary>
    /// <param name="area"></param>
    public void CreateWorldGraphData(AreaBaseData area)
    {
        int nodeCount = UnityEngine.Random.Range(area.minNodeCount, area.maxNodeCount);
        Node<WorldGraphNode>[] nodes = new Node<WorldGraphNode>[nodeCount];
        int indexEnd = nodes.Length - 1;

        EventBaseData[] eventDatas = GenEventDatasForArea(nodeCount - 1, area);

        for (int i = 0; i < nodeCount - 1; i++)
        {
            //if (i == indexEnd)
            //{
            //    //终点
            //    nodes[i] = new Node<WorldGraphNode>(new WorldGraphNode(GenEventTreeHandler(EventDataer.Inst.Get(GameCfg.ID_NEXT_AREA))));
            //}else
            //{
            //    //设置节点事件
            //    nodes[i] = new Node<WorldGraphNode>(new WorldGraphNode(GenEventTreeHandler(EventDataer.Inst.GetARandomRootEvent())));
            //}

            nodes[i] = new Node<WorldGraphNode>(new WorldGraphNode(GenEventTreeHandler(eventDatas[i])));


            if (i == 0)
            {
                //第一个点是可达的
                nodes[i].Data.arrivable = true;
            }
        }

        //设置终点
        nodes[nodeCount - 1] = new Node<WorldGraphNode>(new WorldGraphNode(GenEventTreeHandler(EventDataer.Inst.Get(GameCfg.ID_NEXT_AREA.ToString()))));

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

    public List<string> GetEventLstVisitedInWorld() 
    {
        return lstEventVisitedInWorld;
    }

    public List<string> GetEventLstVisitedInArea()
    {
        return lstEnentVisitedInCurArea;
    }

    /// <summary>
    /// 生成区域的所有事件
    /// </summary>
    /// <returns></returns>
    private EventBaseData[] GenEventDatasForArea(int nodeCount, AreaBaseData areaData)
    {
        //TODO 生成区域的所有事件
        EventFilter filterDef = new EventFilterInTypeAndLevel(100, "Enemy", areaData.level);
        EventBaseData[] evenets = new EventBaseData[nodeCount];
        var filters = areaData.GetEventFilters();
        if (areaData.mode == EEventFilterMode.ALL_APPLY)
        {
            //所有节点都应用第一个过滤器
            var filter = filters[0];
            for (int i = 0; i < nodeCount; i++)
            {
                EventBaseData eventData = null;
                if (filter.CheckOdds())
                {
                    //过滤器生效
                    eventData = filter.GetData();
                }

                if (eventData == null)
                {
                    //使用默认过滤
                    eventData = filterDef.GetData();
                }
                if (eventData != null)
                {
                    evenets[i] = eventData;
                    lstEnentVisitedInCurArea.Add(eventData.ID);
                    lstEventVisitedInWorld.Add(eventData.ID);
                }
                else
                {
                    Debug.LogError("非法的事件生成");
                }
            }
        }
        else if (areaData.mode == EEventFilterMode.IN_ORDER)
        {
            //依顺序应用，然后打乱顺序
            List<EventBaseData> lst = new List<EventBaseData>();
            for (int i = 0; i < nodeCount; i++) 
            {
                EventFilter filter = null;
                if (i < filters.Length)
                {
                    filter = filters[i];
                }
                if (filter == null || !filter.CheckOdds())
                {
                    filter = filterDef;
                }
                var eventData = filter.GetData();
                if (eventData != null)
                {
                    lst.Add(eventData);
                    lstEnentVisitedInCurArea.Add(eventData.ID);
                    lstEventVisitedInWorld.Add(eventData.ID);
                }
                else
                {
                    Debug.LogError("非法的事件生成");
                }
            }
            //打乱顺序
            var lstNew = GameUtil.RandomSortList(lst);
            evenets = lstNew.ToArray();
        }
        return evenets;
    }

    public EventBaseData GetEventBaseData()
    {
        return EventDataer.Inst.Get("1");
    }

    internal void Init(int[] roles, int worldLevel)
    {
        InitCharacterData(roles);
        this.layer = 1;
        this.maxLayer = 10;
        curPointIndex = -1;
        this.worldLevel = worldLevel;
        GenANewAreaData();
    }

    public void Clear() 
    {
        lstAreaVisited.Clear();
        lstEnentVisitedInCurArea.Clear();
        lstEventVisitedInWorld.Clear();
    }

    /// <summary>
    /// 进入一个新区域,重置一些数据
    /// </summary>
    public void ResetOnIntoAArea()
    {
        curPointIndex = -1;
        lstEnentVisitedInCurArea.Clear();
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

    /// <summary>
    /// 生成新的区域数据
    /// </summary>
    public void GenANewAreaData()
    {
        var areaLevel = CalNextAreaLevel();
        var areaData = GetNextAreaData(areaLevel, lstAreaVisited);
        CreateWorldGraphData(areaData);
        mCurArea = areaData;
        lstAreaVisited.Add(areaData.ID);
    }



    /// <summary>
    /// 筛选下一个区域
    /// </summary>
    /// <param name="areaLevel"></param>
    /// <returns></returns>
    private AreaBaseData GetNextAreaData(int areaLevel, List<int> lstExclude)
    {
        return AreaDataer.Inst.GetARandomAreaByLevelAndExclude(areaLevel, lstExclude);        
    }

    /// <summary>
    /// 计算区域等级
    /// </summary>
    /// <returns></returns>
    public int CalNextAreaLevel()
    {
        //TODO 区域等级==世界等级
        return worldLevel;
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