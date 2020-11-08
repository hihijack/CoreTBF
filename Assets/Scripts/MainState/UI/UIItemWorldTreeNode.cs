using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class UIItemWorldTreeNode : UIItemBase
{
    public Image icon;

    WorldGraphNode node;
    private int maxNodeCount;
    private int index;
    RectTransform rectTf;

    bool enableArrive;//允许到达

    public void Init(WorldGraphNode treeNode, int index, int maxNodeCount, bool enableArrive)
    {
        this.node = treeNode;
        this.maxNodeCount = maxNodeCount;
        this.index = index;
        this.enableArrive = enableArrive;
        var btn = icon.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        // UIMgr.Inst.HideUI(UITable.EUITable.UIWorldTree);
        // UIMgr.Inst.HideUI(UITable.EUITable.UIWorldInfo);
        // GameMgr.Inst.ToState(EGameState.Fight);
        if (enableArrive)
        {
            WorldRaidData.Inst.curPointIndex = index;
            UIWorldTree.Inst.RefreshPoint();
            node.eventTreeHandler.TriRoot();
        }
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
        icon.SetSprite(node.eventTreeHandler.tree.root.Data.GetIcon());
        rectTf.anchoredPosition = CalPos();
        if (enableArrive)
        {
            icon.color = Color.white;
        }else
        {
            icon.color = Color.gray;
        }
    }

    private Vector2 CalPos()
    {
          float r = 250;
          float rad = -0.5f * Mathf.PI +  2 * Mathf.PI / maxNodeCount * index;
          return new Vector2(r * Mathf.Cos(rad), r * Mathf.Sin(rad));       
    }
}
