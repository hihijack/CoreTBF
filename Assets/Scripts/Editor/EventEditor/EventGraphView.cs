using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class EventGraphView : GraphView
{
    EditorWindow _win;
    public EventGraphView(EditorWindow win)
    {
        _win = win;
        //按照父级的宽高全屏填充
        this.StretchToParentSize();
        //滚轮缩放
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        //graphview窗口内容的拖动
        this.AddManipulator(new ContentDragger());
        //选中Node移动功能
        this.AddManipulator(new SelectionDragger());
        //多个node框选功能
        this.AddManipulator(new RectangleSelector());
    }

    public EventNodeView AddNodeView(TreeNode<EventBaseData> treeNode) 
    {
        var node = new EventNodeView(treeNode);
        this.AddElement(node);
        return node;
    }

    public EventNodeView AddNodeView(EventBaseData data)
    {
        var node = new EventNodeView(data);
        this.AddElement(node);
        return node;
    }

    public override List<Port> GetCompatiblePorts(Port startAnchor, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();
        var lstPorts = ports.ToList();
        foreach (var port in lstPorts)
        {
            if (startAnchor.node == port.node ||
                startAnchor.direction == port.direction ||
                startAnchor.portType != port.portType)
            {
                continue;
            }

            compatiblePorts.Add(port);
        }
        Debug.Log("GetCompatiblePorts:" + compatiblePorts.Count);
        return compatiblePorts;
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        base.BuildContextualMenu(evt);
        evt.menu.AppendAction("新增节点", OnCreateNode);
        evt.menu.AppendAction("新增预设节点", OnCreatePresetNode);
    }

    private void OnCreatePresetNode(DropdownMenuAction obj)
    {
        EditorWinEventEditor.Inst.OnStartCreatPresetNode(obj);
    }

    /// <summary>
    /// 新增节点
    /// </summary>
    /// <param name="obj"></param>
    private void OnCreateNode(DropdownMenuAction action)
    {
        EditorWinEventEditor.Inst.OnCreatNode(action);
    }
}