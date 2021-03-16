using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using System.Text;
using UnityEngine.UIElements;
using System;
using SimpleJSON;
using System.Collections.Generic;

[System.Serializable]
public class PortsDataContent 
{
    public List<PortData> lstProtDatas;
}

[System.Serializable]
public class PortData 
{
    public string protDesc;
}

public class EventNodeView : Node
{
    TreeNode<EventBaseData> _treeNode;
    EventBaseData _eventData;

    Dictionary<string, Port> _dicEventPortMap;

    Port _inputPort;

    public Vector2 pos;

    List<PortInfoData> _lstProtInfoData;

    Dictionary<PortInfoData, Port> _dicPortMap;

    /// <summary>
    /// 是否新增的
    /// </summary>
    public bool isNewAdded = false;

    private bool _modifyDirtyFlag = false;

    public EventNodeView(TreeNode<EventBaseData> treeNode) 
   {
        _eventData = treeNode.Data;
        _treeNode = treeNode;
        Init();  
    }

    public EventNodeView(EventBaseData data)
    {
        _eventData = data;
        Init();
    }

    public void MarkModifyDirtyFlag(bool modifyed)
    {
        _modifyDirtyFlag = modifyed;
    }

    public bool IsModifyedDirty()
    {
        return _modifyDirtyFlag;
    }

    void Init()
    {
        _dicEventPortMap = new Dictionary<string, Port>();
        title = $"ID:{_eventData.ID}";

        //创建一个inputPort
        _inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(Port));
        //设置port显示的名称
        _inputPort.portName = _eventData.desc;
        //添加到inputContainer容器中
        inputContainer.Add(_inputPort);

        //添加选项
        AddOptPorts();

        //this.focusable = true;
        //this.RegisterCallback<FocusOutEvent>(OnFocusOut);
        //this.RegisterCallback<FocusInEvent>(OnFocusIn);
    }

    internal void DeletePortInfoData(PortInfoData data)
    {
        var port = _dicPortMap[data];
        if (port != null)
        {
            port.DisconnectAll();
            _dicPortMap.Remove(data);
            outputContainer.Remove(port);
            _lstProtInfoData.Remove(data);
        }
    }

    internal void MovePortInfoDtatDown(PortInfoData data)
    {
        var nextPortData = GetPortInfoDataByIndex(data.index + 1);
        if (nextPortData != null)
        {
            data.index++;
            nextPortData.index--;
            outputContainer.Sort(SortPort);
        }
    }

    /// <summary>
    /// 上移
    /// </summary>
    /// <param name="data"></param>
    internal void MovePortInfoDataUp(PortInfoData data)
    {
        var perPortData = GetPortInfoDataByIndex(data.index - 1);
        if (perPortData != null)
        {
            data.index --;
            perPortData.index ++;
            outputContainer.Sort(SortPort);        
        }
    }

    private int SortPort(VisualElement x, VisualElement y)
    {
        var portX = x as Port;
        var portY = y as Port;
        PortInfoData pidX = portX.userData as PortInfoData;
        PortInfoData pidY = portY.userData as PortInfoData;
        return pidX.index - pidY.index;
    }

    private PortInfoData GetPortInfoDataByIndex(int index)
    {
        foreach (var infoData in _lstProtInfoData)
        {
            if (infoData.index == index)
            {
                return infoData;
            }
        }
        return null;
    }

    internal void SetPortInfoDataDesc(PortInfoData data, string desc)
    {
        data.desc = desc;
        if (_dicPortMap.ContainsKey(data))
        {
            var port = _dicPortMap[data];
            port.portName = data.desc;
        }
    }

    public List<PortInfoData> GetProtsLst()
    {
        if (_lstProtInfoData == null)
        {
            _lstProtInfoData = new List<PortInfoData>();
            _dicPortMap = new Dictionary<PortInfoData, Port>();
            for (int i = 0; i < outputContainer.childCount; i++)
            {
                Port port = outputContainer[i] as Port;
                PortInfoData pid = new PortInfoData();
                pid.index = i;
                pid.desc = port.portName;
                _lstProtInfoData.Add(pid);
                _dicPortMap.Add(pid, port);
                port.userData = pid;
            }
        }
        return _lstProtInfoData;
    }

    internal void RefreshDesc()
    {
        _inputPort.portName = _eventData.desc;
    }

    public List<PortInfoData> GetSortedProtsLst()
    {
        var lst = GetProtsLst();
        lst.Sort(SortProtInfoData);
        return lst;
    }

    private int SortProtInfoData(PortInfoData x, PortInfoData y)
    {
        return x.index - y.index;
    }

    public override void OnSelected()
    {
        base.OnSelected();
        EditorWinEventEditor.Inst.OnNodeFocus(this, true);
    }

    public override void OnUnselected()
    {
        base.OnUnselected();
        EditorWinEventEditor.Inst.OnNodeFocus(this, false);
    }

    void AddOptPorts() 
    {
        if (_eventData.lstChildID != null)
        {
            for (int i = 0; i < _eventData.lstChildID.Count; i++)
            {
                string eventID = _eventData.lstChildID[i];
                //是否有选项
                string optDesc = "";
                if (_eventData.lstOptions != null && i < _eventData.lstOptions.Count)
                {
                    optDesc = _eventData.lstOptions[i];
                }
               
                var outPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(Port));
                outPort.portName = optDesc;
                outputContainer.Add(outPort);
                _dicEventPortMap.Add(eventID, outPort);
            }
        }
        RefreshExpandedState();
    }

    internal void AddANewPort()
    {
        var outPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(Port));
        outPort.portName = "";
        outputContainer.Add(outPort);
        PortInfoData pid = new PortInfoData();
        pid.index = _lstProtInfoData.Count;
        pid.desc = outPort.portName;
        _lstProtInfoData.Add(pid);
        _dicPortMap.Add(pid, outPort);
        outPort.userData = pid;
        RefreshExpandedState();
    }

    private void OnFocusIn(FocusInEvent evt)
    {
        EditorWinEventEditor.Inst.OnNodeFocus(this, true);
    }

    private void OnFocusOut(FocusOutEvent evt)
    {
        EditorWinEventEditor.Inst.OnNodeFocus(this, false);
    }

    public string GetTitleInfo()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"ID:{_eventData.ID}");
        sb.AppendLine(_eventData.desc);
        return sb.ToString();
    }

    public EventBaseData GetData()
    {
        return _eventData;
    }

    public TreeNode<EventBaseData> GetTreeNode()
    {
        return _treeNode;
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        base.BuildContextualMenu(evt);
        //evt.menu.AppendAction("删除", OnDeleteNode);
    }

    /// <summary>
    /// 更新当前Data:更新连接的节点ID列表
    /// </summary>
    internal void UpdateData()
    {
        List<EventNodeView> lstOutNodeViews = this.GetTargetConnectedNodeViewList();
        if (lstOutNodeViews != null && lstOutNodeViews.Count > 0)
        {
            if (_eventData.lstChildID == null)
            {
                _eventData.lstChildID = new List<string>();
            }
            else
            {
                _eventData.lstChildID.Clear();
            }
            for (int i = 0; i < lstOutNodeViews.Count; i++)
            {
                _eventData.lstChildID.Add(lstOutNodeViews[i].GetData().ID);
            }
        }
        else
        {
            _eventData.lstChildID = null;
        }

        //更新选项
        if (outputContainer.childCount > 0)
        {
            if (_eventData.lstOptions == null)
            {
                _eventData.lstOptions = new List<string>();
            }
            else
            {
                _eventData.lstOptions.Clear();
            }
            for (int i = 0; i < outputContainer.childCount; i++)
            {
                var port = outputContainer[i] as Port;
                if (!string.IsNullOrEmpty(port.portName))
                {
                    _eventData.lstOptions.Add(port.portName);
                }
            }
        }
        else
        {
            _eventData.lstOptions = null;
        }

    }

    /// <summary>
    /// 取连接的节点列表
    /// </summary>
    /// <returns></returns>
    internal List<EventNodeView> GetTargetConnectedNodeViewList()
    {
        if (outputContainer.childCount == 0)
        {
            return null;
        }
        List<EventNodeView> lst = new List<EventNodeView>();
        for (int i = 0; i < outputContainer.childCount; i++)
        {
            Port port = outputContainer[i] as Port;
            var edges = port.connections;
            foreach (var edge in edges)
            {
                lst.Add(edge.input.node as EventNodeView);
            }
        }
        return lst;
    }

    internal Port GetPort(string eventID)
    {
        return _dicEventPortMap[eventID];
    }

    internal Port GetInputPort()
    {
        return _inputPort;
    }
}