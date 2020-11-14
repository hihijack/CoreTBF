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

    Action<int,WorldGraphNode> cbOnNodeItemClick;

    public void Init(WorldGraphNode treeNode, int index, int maxNodeCount, Action<int,WorldGraphNode> cbOnNodeItemClick)
    {
        this.node = treeNode;
        this.maxNodeCount = maxNodeCount;
        this.index = index;
        this.cbOnNodeItemClick = cbOnNodeItemClick;
        var btn = icon.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        // UIMgr.Inst.HideUI(UITable.EUITable.UIWorldTree);
        // UIMgr.Inst.HideUI(UITable.EUITable.UIWorldInfo);
        // GameMgr.Inst.ToState(EGameState.Fight);
        cbOnNodeItemClick(index,node);
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
        if (node.hasClear)
        {
            //已清理
            icon.SetSprite("Buffs/Icon1_96");
        }else
        {
            icon.SetSprite(node.eventTreeHandler.tree.root.Data.GetIcon());
        }
        
        rectTf.anchoredPosition = CalPos();
        if (node.arrivable)
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
