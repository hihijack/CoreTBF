using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemWorldTreeNode : UIItemBase
{
    public Image icon;

    WorldTreeNode treeNode;

    RectTransform rectTf;

    public void Init(WorldTreeNode treeNode)
    {
        this.treeNode = treeNode;
    }

    public override void OnAwake()
    {
        base.OnAwake();
        rectTf = GetComponent<RectTransform>();
    }

    public Vector2 GetPos()
    {
        return rectTf.anchoredPosition;
    }

    public override void Refresh()
    {
        base.Refresh();
        icon.SetSprite(treeNode.eventBaseData.GetIcon());
        rectTf.anchoredPosition = CalPos();
        name = treeNode.layer + "_" + treeNode.index;
    }

    private Vector2 CalPos()
    {
        Vector2 r = Vector2.zero;
        float yInter = 200;
        float xInter = 150;
        r.y = 250 + treeNode.layer * yInter + UnityEngine.Random.Range(-30f,30f);
        int iOffsetCenter = GetCenterIndex(treeNode.index, treeNode.maxIndexCurLayer);
        r.x = iOffsetCenter * xInter + UnityEngine.Random.Range(-45f, 45f);
        return r;
    }

    int GetCenterIndex(int i, int count)
    {
        int r = 0;
        if (count % 2 == 0)
        {
            if (i >= count / 2)
            {
                r = i - count / 2 + 1;
            }
            else
            {
                r = i - count / 2;
            }
        }
        else
        {
            r = i - count / 2;
        }
        return r;
    }
}
