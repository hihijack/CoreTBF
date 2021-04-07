using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SimpleJSON;
using Sirenix.Serialization;

public class UIWorldTree : UIBase
{
    public Transform tfTreeNodes;
    public Transform tfLines;
    public GameObject pfbItem;
    public GameObject pfbItemLine;
    public Image imgPoint;

    Dictionary<WorldGraphNode, UIItemWorldTreeNode> _dic;
    List<UIItemLine> _lstLines;

    public static UIWorldTree Inst;

    public override void Init()
    {
        base.Init();
        Inst = this;

        _dic = new Dictionary<WorldGraphNode, UIItemWorldTreeNode>();
        _lstLines = new List<UIItemLine>();
    }

    /// <summary>
    /// 当前节点标记为清理
    /// </summary>
    /// <param name="data"></param>
    private void OnEventNodeMarkAsCleared(object data)
    {
        var curNode = WorldRaidData.Inst.GetCurInTreeNode();
        if (_dic.ContainsKey(curNode))
        {
            var uiItem = _dic[curNode];
            uiItem.Refresh();
        }
    }

    public override void OnShow()
    {
        base.OnShow();
        Event.Inst.Register(Event.EEvent.WORLD_NODE_MARK_CLEAR, OnEventNodeMarkAsCleared);
        Event.Inst.Register(Event.EEvent.WORLD_TREE_STATE_UPDATE, OnEventWorldTreeStateUpdate);
        Refresh();
    }

    /// <summary>
    /// 世界树状态刷新
    /// </summary>
    /// <param name="obj"></param>
    private void OnEventWorldTreeStateUpdate(object obj)
    {
        foreach (var uiItem in _dic.Values)
        {
            uiItem.Refresh();
        }
        foreach (var itemLine in _lstLines)
        {
            itemLine.Refresh();
        }
    }

    public override void OnHide()
    {
        base.OnHide();
        Event.Inst.UnRegister(Event.EEvent.WORLD_NODE_MARK_CLEAR, OnEventNodeMarkAsCleared);
         Event.Inst.UnRegister(Event.EEvent.WORLD_TREE_STATE_UPDATE, OnEventWorldTreeStateUpdate);
        Clear();
    }

    void Clear()
    {
        foreach (var item in _dic.Values)
        {
            item.Cache();
        }
        _dic.Clear();
        foreach (var item in _lstLines)
        {
            item.Cache();
        }
        _lstLines.Clear();
    }

    public void Refresh()
    {
        Clear();
        StartCoroutine(CoShowTree());
    }

    IEnumerator CoShowTree()
    {
        int numOfNode = WorldRaidData.Inst.graph.GetNumOfVertex();
        for (int i = 0; i < numOfNode; i++)
        {
            var vertNode = WorldRaidData.Inst.graph[i];
            var uiItem = UIItemBase.Create<UIItemWorldTreeNode>(tfTreeNodes, pfbItem);
            uiItem.Init(vertNode.Data.Data, i , numOfNode, OnClickNodeItem);
            uiItem.Refresh();
            _dic.Add(vertNode.Data.Data, uiItem);
            yield return 0;
        }
        
         for (int i = 0; i < numOfNode; i++)
        {
            var vertNode = WorldRaidData.Inst.graph[i];
            var next = vertNode.FirstAdj;
            while (next != null)
            {
                var nextIndex = next.Adjvex;
                if (nextIndex > i)
                {
                    var vertNodeOther = WorldRaidData.Inst.graph[nextIndex];
                    var uiItem = _dic[vertNode.Data.Data];
                    var uiItemTarget = _dic[vertNodeOther.Data.Data];
                    var uiItemLine = UIItemBase.Create<UIItemLine>(tfLines, pfbItemLine);
                    bool enableArrive = vertNodeOther.Data.Data.arrivable;
                    uiItemLine.Init(uiItem.GetPos(), uiItemTarget.GetPos(), vertNode.Data.Data, vertNodeOther.Data.Data);
                    uiItemLine.Refresh();
                    _lstLines.Add(uiItemLine);
                    yield return 0;
                }
                next = next.Next;
            }
        }
        //显示角色位置
        RefreshPoint();
    }

    private void OnClickNodeItem(int index, WorldGraphNode node)
    {
        WorldRaidData.Inst.DebugSetWorldGraphNode(node);

        if (node.arrivable)
        {
            // WorldRaidData.Inst.curPointIndex = index;
            WorldRaidData.Inst.SetCurPointIndex(index);
            //食物消耗
            PlayerDataMgr.Inst.PlayerData.ChangeItem(GameCfg.ID_FOOD, -1);
            Event.Inst.Fire(Event.EEvent.PlayerItemChange, null);
            UIWorldTree.Inst.RefreshPoint();
            if (!node.hasClear)
            {
              node.eventTreeHandler.TriRoot();  
            }
        }
    }

    public void RefreshPoint()
    {
        var curInNode = WorldRaidData.Inst.GetCurInTreeNode();
        if (curInNode != null)
        {
            var uiItemCurIn = _dic[curInNode];
            imgPoint.rectTransform.anchoredPosition = uiItemCurIn.GetPos();
        }else
        {
            imgPoint.rectTransform.anchoredPosition = Vector3.right * 10000;
        }
    }
}
