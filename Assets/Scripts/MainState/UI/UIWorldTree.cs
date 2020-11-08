using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SimpleJSON;

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

        EventProcessor.Inst.RegistorEvent(EventProcessor.EVENT_SHOW, OnEventShowEventUI);
        EventProcessor.Inst.RegistorEvent(EventProcessor.EVENT_FIGHT, OnEventFight);
        EventProcessor.Inst.RegistorEvent(EventProcessor.EVENT_LEAVE_WORLD, OnEventLeaveWorld);
    }

    public override void OnShow()
    {
        base.OnShow();

        StartCoroutine(CoShowTree());
    }

    public override void OnHide()
    {
        base.OnHide();
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

    private void OnEventLeaveWorld(EventBaseData eventBaseData, JSONNode data)
    {
         WorldRaidMgr.Inst.OnEventLeaveWorld(eventBaseData, data);
    }

    private void OnEventFight(EventBaseData eventBaseData, JSONNode data)
    {
        WorldRaidMgr.Inst.OnEventFight(eventBaseData, data);
    }

    private void OnEventShowEventUI(EventBaseData eventBaseData, JSONNode data)
    {
        WorldRaidMgr.Inst.OnEventShowEventUI(eventBaseData, data);
    }

    IEnumerator CoShowTree()
    {
        _dic.Clear();
        var lstEnableTreeNodeIndexs = WorldRaidData.Inst.GetEnableTreeNodeIndexs();
        int numOfNode = WorldRaidData.Inst.graph.GetNumOfVertex();
        for (int i = 0; i < numOfNode; i++)
        {
            var vertNode = WorldRaidData.Inst.graph[i];
            var uiItem = UIItemBase.Create<UIItemWorldTreeNode>(tfTreeNodes, pfbItem);
            bool enableArrive = lstEnableTreeNodeIndexs.Contains(i);
            uiItem.Init(vertNode.Data.Data, i , numOfNode, enableArrive);
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
                    bool enableArrive = WorldRaidData.Inst.curPointIndex == i && lstEnableTreeNodeIndexs.Contains(nextIndex);
                    uiItemLine.Init(uiItem.GetPos(), uiItemTarget.GetPos(),enableArrive);
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
