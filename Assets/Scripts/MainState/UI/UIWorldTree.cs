using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIWorldTree : UIBase
{
    public Transform tfTreeNodes;
    public Transform tfLines;
    public GameObject pfbItem;
    public GameObject pfbItemLine;
    public Image imgPoint;

    Dictionary<WorldGraphNode, UIItemWorldTreeNode> _dic;
    List<UIItemLine> _lstLines;

    public override void Init()
    {
        base.Init();
        _dic = new Dictionary<WorldGraphNode, UIItemWorldTreeNode>();
        _lstLines = new List<UIItemLine>();
    }

    public override void OnShow()
    {
        base.OnShow();

        StartCoroutine(CoShowTree());
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
