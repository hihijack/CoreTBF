using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System;
using UnityEditor.UIElements;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Experimental.PlayerLoop;
using Sirenix.Utilities;
using SimpleJSON;
using UnityEditor.Experimental.GraphView;


public class PortInfoData 
{
    public int index;
    public string desc;
}

public class EditorWinEventEditor : EditorWindow
{

    public static EditorWinEventEditor Inst;

    EventGraphView _graphView;

    Box _boxInspector;

    PortsListView _portListView;
    
    /// <summary>
    /// 当前选中节点
    /// </summary>
    EventNodeView _curFocusNodeView;

    /// <summary>
    /// <事件ID,节点>
    /// </summary>
    Dictionary<string, EventNodeView> _dicNodeViewMap; 

    bool _flagRefreshNodeViewPos;
    int _frameRefresh;

    Vector2 _posCacheToAddPerset;

    [MenuItem("Tools/事件编辑器")]
    static void ShowWin()
    {
        EditorWinEventEditor win = GetWindow<EditorWinEventEditor>(false, "事件编辑器");
        win.minSize = new Vector2(1600, 800);
    }

    private void OnEnable()
    {
        Inst = this;
        _dicNodeViewMap = new Dictionary<string, EventNodeView>();
        InitData();
        InitUI();
    }

    private void Update()
    {
       
    }

    private void OnInspectorUpdate()
    {
        //Node的Position并不会马上更新，放到这边以获取正确的Position信息
        if (_flagRefreshNodeViewPos)
        {
            _flagRefreshNodeViewPos = false;
            AdjustNodeViewLayout();
        }
    }

    private void InitUI()
    {
        string uxmlPath = "Assets/Scripts/Editor/EventEditor/winEventEditor.uxml";
        VisualTreeAsset uiAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxmlPath);
        VisualElement ui = uiAsset.CloneTree("");
        rootVisualElement.Add(ui);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/EventEditor/stylesWinEventEditor.uss");
        rootVisualElement.styleSheets.Add(styleSheet);

        var grahpViewBox = rootVisualElement.Q("graphview");
        _graphView = new EventGraphView(this);
        grahpViewBox.Add(_graphView);

        //按键监听
        var btnOpen = rootVisualElement.Q<Button>("open");
        btnOpen.RegisterCallback<MouseCaptureEvent>(OnBtnOpen);


        var btnSave = rootVisualElement.Q<Button>("save");
        btnSave.RegisterCallback<MouseCaptureEvent>(OnBtnSave);

        RefreshNodeInfo();

        var btnAddPort = rootVisualElement.Q<Button>("addPort");
        btnAddPort.RegisterCallback<MouseCaptureEvent>(OnBtnAddPort);

        

        //信息栏值变化监听
        if (_boxInspector == null)
        {
            _boxInspector = rootVisualElement.Q<Box>("inspector");
        }
        Toggle togPerset = _boxInspector.Q<Toggle>("isPresetEvent");
        togPerset.RegisterValueChangedCallback(OnTogPerSetChangeed);
        EnumField enumField = _boxInspector.Q<EnumField>("txtEventType");
        enumField.RegisterValueChangedCallback(OnEunmEventTyoeChanged);
        TextField txtEvent = _boxInspector.Q<TextField>("txtEventEvent");
        txtEvent.RegisterValueChangedCallback(OnTexEvetnEventChanged);
        Toggle togRoot = _boxInspector.Q<Toggle>("isEventRoot");
        togRoot.RegisterValueChangedCallback(OnTogIsEventRootChanged);
        IntegerField intFieldLevel = _boxInspector.Q<IntegerField>("txtLevel");
        intFieldLevel.RegisterValueChangedCallback(OnTxtLevelChanged);
        Toggle togRepetInArea = _boxInspector.Q<Toggle>("isEnableRepetInArea");
        togRepetInArea.RegisterValueChangedCallback(OnTogRepetInAreaChanged);
        Toggle togRepetWorld = _boxInspector.Q<Toggle>("isEnableRepetInWorld");
        togRepetWorld.RegisterValueChangedCallback(OnTogRepetInWorldChanged);
        TextField txtDesc = _boxInspector.Q<TextField>("txtEventDesc");
        txtDesc.RegisterValueChangedCallback(OnTxtEventDescChanged);
    }

    private void OnTxtEventDescChanged(ChangeEvent<string> evt)
    {
        if (_curFocusNodeView == null)
        {
            return;
        }
        var data = _curFocusNodeView.GetData();
        data.desc = evt.newValue;
        _curFocusNodeView.RefreshDesc();
    }

    private void OnTogRepetInWorldChanged(ChangeEvent<bool> evt)
    {
        if (_curFocusNodeView == null)
        {
            return;
        }
        var data = _curFocusNodeView.GetData();
        data.enableRepsetInWorld = evt.newValue;
    }

    private void OnTogRepetInAreaChanged(ChangeEvent<bool> evt)
    {
        if (_curFocusNodeView == null)
        {
            return;
        }
        var data = _curFocusNodeView.GetData();
        data.enableRepetInArea = evt.newValue;
    }

    private void OnTxtLevelChanged(ChangeEvent<int> evt)
    {
        if (_curFocusNodeView == null)
        {
            return;
        }
        var data = _curFocusNodeView.GetData();
        data.level = evt.newValue;
    }

    private void OnTogIsEventRootChanged(ChangeEvent<bool> evt)
    {
        if (_curFocusNodeView == null)
        {
            return;
        }
        var data = _curFocusNodeView.GetData();
        data.isRoot = evt.newValue;
    }

    private void OnTexEvetnEventChanged(ChangeEvent<string> evt)
    {
        if (_curFocusNodeView == null)
        {
            return;
        }
        var data = _curFocusNodeView.GetData();
        if (string.IsNullOrEmpty(evt.newValue))
        {
            data.jsonEvents = null;
        }
        else
        {
            data.jsonEvents = JSON.Parse(evt.newValue);
        }
    }

    private void OnEunmEventTyoeChanged(ChangeEvent<Enum> evt)
    {
        if (_curFocusNodeView == null)
        {
            return;
        }
        var data = _curFocusNodeView.GetData();
        data.type = (EEventType)evt.newValue;
    }

    private void OnTogPerSetChangeed(ChangeEvent<bool> evt)
    {
        if (_curFocusNodeView == null)
        {
            return;
        }
        var data = _curFocusNodeView.GetData();
        data.preset = evt.newValue;
    }

    /// <summary>
    /// 新增port
    /// </summary>
    /// <param name="evt"></param>
    private void OnBtnAddPort(MouseCaptureEvent evt)
    {
        if (_curFocusNodeView == null)
        {
            return;
        }
        _curFocusNodeView.AddANewPort();
        _portListView.Refresh();
    }


    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="evt"></param>
    private void OnBtnSave(MouseCaptureEvent evt)
    {
        foreach (var nodeView in _dicNodeViewMap.Values)
        {
            nodeView.UpdateData();
            EventDataer.Inst.UpdateToDB(nodeView.GetData(), nodeView.isNewAdded);
        }

    }

    private void OnBtnOpen(MouseCaptureEvent evt)
    {
        //Debug.Log("open");
        var winRootEventLst = GetWindow<EditorWinEventSelector>("事件选择");
        winRootEventLst.Init(OnSelectEventToOpen, this);
    }

    /// <summary>
    /// 创建新节点
    /// </summary>
    internal void OnCreatNode(DropdownMenuAction actionObj)
    {
        var eventDataNew = EventDataer.Inst.NewData();
        var nodeView = _graphView.AddNodeView(eventDataNew);
        nodeView.isNewAdded = true;
        _dicNodeViewMap.Add(eventDataNew.ID, nodeView);
        nodeView.SetPosition(nodeView.GetPosition().SetPosition(actionObj.eventInfo.localMousePosition));
    }

    /// <summary>
    /// 创建预设节点
    /// </summary>
    /// <param name="action"></param>
    internal void OnStartCreatPresetNode(DropdownMenuAction action)
    {
        var winRootEventLst = GetWindow<EditorWinEventPersetSelector>("预设事件选择");
        winRootEventLst.Init(OnSelectPersetEventToAdd, this);
        _posCacheToAddPerset = action.eventInfo.localMousePosition;
    }

    /// <summary>
    /// 选择一个预设事件添加
    /// </summary>
    /// <param name="obj"></param>
    private void OnSelectPersetEventToAdd(EventBaseData eventData)
    {
        var nodeView = _graphView.AddNodeView(eventData);
        _dicNodeViewMap.Add(eventData.ID, nodeView);
        nodeView.SetPosition(nodeView.GetPosition().SetPosition(_posCacheToAddPerset));
    }

    private void OnSelectEventToOpen(EventBaseData data)
    {
        Debug.Log("打开事件:" + data.desc);

        //_graphView.Clear();
        //删除node
        var children = _graphView.nodes.ToList();
        _graphView.DeleteElements(children);
        //删除连线
        var edges = _graphView.edges.ToList();
        _graphView.DeleteElements(edges);
        _dicNodeViewMap.Clear();
        //初始化树
        Tree<EventBaseData> tree = new Tree<EventBaseData>(new TreeNode<EventBaseData>(data));
        AddChildForTreeNode(tree.root);
        //遍历生成节点
        tree.Iterator(GenNodeView);
        _flagRefreshNodeViewPos = true;
        _frameRefresh = Time.renderedFrameCount;
        //连接线
        GenConnectEdges();
    }

    private void GenConnectEdges()
    {
        foreach (var nodeView in _dicNodeViewMap.Values)
        {
            List<string> lstChildID = nodeView.GetData().lstChildID;
            if (lstChildID == null)
            {
                continue;
            }
            for (int i = 0; i < lstChildID.Count; i++)
            {
                string childEventID = lstChildID[i];
                Port portFrom = nodeView.GetPort(childEventID);
                var nodeViewTarget = GetNodeView(childEventID);
                if (nodeViewTarget != null)
                {
                    Port portTarget = nodeViewTarget.GetInputPort();
                    var edge = portFrom.ConnectTo(portTarget);
                    _graphView.AddElement(edge);
                }
            }
        }
    }

    /// <summary>
    /// 节点选中/取消选择
    /// </summary>
    /// <param name="eventNodeView"></param>
    /// <param name="v"></param>
    internal void OnNodeFocus(EventNodeView eventNodeView, bool focus)
    {
        if (focus)
        {
            _curFocusNodeView = eventNodeView;
        }
       
        //刷新节点信息
        RefreshNodeInfo();
    }

    private void RefreshNodeInfo()
    {
        if (_boxInspector == null)
        {
            _boxInspector = rootVisualElement.Q<Box>("inspector");
        }
        if (_curFocusNodeView != null)
        {
             _boxInspector.visible = true;
            var data = _curFocusNodeView.GetData();
            Label labID = _boxInspector.Q<Label>("txtEventID");
            labID.text = data.ID.ToString();
            Toggle togPerset = _boxInspector.Q<Toggle>("isPresetEvent");
            togPerset.value = data.preset;
            EnumField enumField = _boxInspector.Q<EnumField>("txtEventType");
            enumField.Init(EEventType.Other);
            enumField.value = data.type;
            TextField txtEvent = _boxInspector.Q<TextField>("txtEventEvent");
            txtEvent.value = data.jsonEvents?.ToString();
            Toggle togRoot = _boxInspector.Q<Toggle>("isEventRoot");
            togRoot.value = data.isRoot;
            IntegerField intFieldLevel = _boxInspector.Q<IntegerField>("txtLevel");
            intFieldLevel.value = data.level;
            Toggle togRepetInArea = _boxInspector.Q<Toggle>("isEnableRepetInArea");
            togRepetInArea.value = data.enableRepetInArea;
            Toggle togRepetWorld = _boxInspector.Q<Toggle>("isEnableRepetInWorld");
            togRepetWorld.value = data.enableRepsetInWorld;
            TextField txtDesc = _boxInspector.Q<TextField>("txtEventDesc");
            txtDesc.value = data.desc;

            if (_portListView == null)
            {
                Box protBox = _boxInspector.Q<Box>("portsBox");
                _portListView = new PortsListView();
                _portListView.name = "portsListView";
                protBox.Add(_portListView);
            }
            _portListView.SetDataSource(_curFocusNodeView);
        }
        else
        {
            _boxInspector.visible = false;
        }
       
    }

    private void BindItemInspectorProtLst(VisualElement element, int index)
    {
        var lab = element as Label;
    }

    private void GenNodeView(TreeNode<EventBaseData> node)
    {
        Debug.Log("GenNodeView:" + node.Data.desc);
        var nodeView = _graphView.AddNodeView(node);
        _dicNodeViewMap.Add(node.Data.ID, nodeView);
    }

    /// <summary>
    /// 调整节点布局
    /// </summary>
    void AdjustNodeViewLayout()
    {
        foreach (var nodeView in _dicNodeViewMap.Values)
        {
            var node = nodeView.GetTreeNode();
            if (node == null)
            {
                continue;
            }
            //取父节点nodeview
            if (node.parent != null)
            {
                string parentNodeID = node.parent.Data.ID;

                var nodeViewParent = GetNodeView(parentNodeID);

                Rect rectParnet = Rect.zero;
                if (nodeViewParent != null)
                {
                    rectParnet = nodeViewParent.GetPosition();
                    rectParnet.position = nodeViewParent.pos;
                }
                else
                {
                    Debug.LogError("Get parent nodeView is null");//######
                }

                //设置位置
                Vector2 pos = rectParnet.position + new Vector2(rectParnet.width + 50, node.index * 150);
                Vector2 size = nodeView.GetPosition().size;
                var rect = new Rect(pos.x, pos.y, size.x, size.y);
                nodeView.SetPosition(rect);
                nodeView.pos = pos;
            }
        }
    }

    private EventNodeView GetNodeView(string nodeID)
    {
        if (_dicNodeViewMap.ContainsKey(nodeID))
        {
            return _dicNodeViewMap[nodeID];
        }
        return null;
    }

    /// <summary>
    /// 递归生产树
    /// </summary>
    /// <param name="treeNode"></param>
    void AddChildForTreeNode(TreeNode<EventBaseData> treeNode)
    {
        if (treeNode.Data.lstChildID == null || treeNode.Data.lstChildID.Count == 0)
        {
            return;
        }

        for (int i = 0; i < treeNode.Data.lstChildID.Count; i++)
        {
            string childId = treeNode.Data.lstChildID[i];
            treeNode.AddChild(new TreeNode<EventBaseData>(EventDataer.Inst.Get(childId)));
        }
        foreach (var child in treeNode.childs)
        {
            AddChildForTreeNode(child);
        }
    }

    private void OnBtnAdd()
    {
        throw new NotImplementedException();
    }


    void InitData() 
    {
        GameData.Inst.Init();
        EventDataer.Inst.InitAllEventData();
    }

    private void OnDestroy()
    {
        GameData.Inst.Release();
    }
}