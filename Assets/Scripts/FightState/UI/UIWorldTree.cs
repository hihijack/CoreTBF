using UnityEngine;
using System.Collections;
using System;
using Boo.Lang;
using System.Collections.Generic;

public class UIWorldTree : UIBase
{
    public Transform tfTreeNodes;
    public Transform tfLines;
    public GameObject pfbItem;
    public GameObject pfbItemLine;

    Dictionary<WorldTreeNode, UIItemWorldTreeNode> _dic;

    public override void Init()
    {
        base.Init();
        _dic = new Dictionary<WorldTreeNode, UIItemWorldTreeNode>();
    }

    public override void OnShow()
    {
        base.OnShow();
        
        if (!WorldTreeData.Inst.HasData())
        {
            WorldTreeData.Inst.CreateData(10);
        }

        StartCoroutine(CoShowTree());
    }

    IEnumerator CoShowTree()
    {
        _dic.Clear();
        for (int i = 0; i < WorldTreeData.Inst.lstNodes.Count; i++)
        {
            var data = WorldTreeData.Inst.lstNodes[i];
            var uiItem = UIItemBase.Create<UIItemWorldTreeNode>(tfTreeNodes, pfbItem);
            uiItem.Init(data);
            uiItem.Refresh();
            _dic.Add(data, uiItem);
            yield return 0;
        }
        //创建连线
        for (int i = 0; i < WorldTreeData.Inst.lstNodes.Count; i++)
        {
            var data = WorldTreeData.Inst.lstNodes[i];
            var uiItem = _dic[data];
            foreach (var childNode in data.childs)
            {
                var uiItemChild = _dic[childNode];
                var uiItemLine = UIItemBase.Create<UIItemLine>(tfLines, pfbItemLine);
                uiItemLine.Init(uiItem.GetPos(), uiItemChild.GetPos());
                uiItemLine.Refresh();
                yield return 0;
            }
        }
    }
}
