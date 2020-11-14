using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemLine : UIItemBase
{
    private Vector2 startPoint;
    private Vector2 endPoint;

    RectTransform rectTf;

    Image image;

    WorldGraphNode nodeP1;
    WorldGraphNode nodeP2;

    public void Init(Vector2 startPoint,Vector2 endPoint, WorldGraphNode nodeP1, WorldGraphNode nodeP2)
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.nodeP1 = nodeP1;
        this.nodeP2 = nodeP2;
    }

    public override void OnAwake()
    {
        base.OnAwake();
        rectTf = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    public override void Refresh()
    {
        base.Refresh();
        rectTf.anchoredPosition = startPoint;
        rectTf.localRotation = Quaternion.LookRotation(Vector3.forward, endPoint - startPoint);
        rectTf.sizeDelta = new Vector2(rectTf.sizeDelta.x, Vector2.Distance(endPoint, startPoint));
        
        var curInNode = WorldRaidData.Inst.GetCurInTreeNode();
        bool enableArrive = GetIsArrive(nodeP1, nodeP2, curInNode) || GetIsArrive(nodeP2, nodeP1, curInNode);
        if (enableArrive)
        {
            image.color = Color.white;
        }else
        {
            image.color = Color.grey;
        }
    }

    private bool GetIsArrive(WorldGraphNode nodeStart, WorldGraphNode nodeEnd, WorldGraphNode curInNode)
    {
        return nodeStart == curInNode && nodeEnd.arrivable;
    }
}
