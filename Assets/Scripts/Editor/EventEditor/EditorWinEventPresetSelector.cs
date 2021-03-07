using UnityEditor;
using System;
using UnityEngine.UIElements;
using Boo.Lang;

/// <summary>
/// 预设事件选择器
/// </summary>
public class EditorWinEventPersetSelector : EditorWindow
{
    Action<EventBaseData> _cbSelect;

    List<EventBaseData> lstDatas;

    EditorWindow _parentWin;

    private void OnEnable()
    {
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/EventEditor/stylesWinEventSelector.uss");
        rootVisualElement.styleSheets.Add(styleSheet);

        lstDatas = new List<EventBaseData>();
        foreach (var eventData in EventDataer.Inst.GetDic().Values)
        {
            if (eventData.preset)
            {
                lstDatas.Add(eventData);
            }
        }
        ListView lstView = new ListView(lstDatas, 30, ItemCreator, BindItem);
        lstView.onItemChosen += onItemChosen;
        rootVisualElement.Add(lstView);
    }

    public void Init(Action<EventBaseData> cbSelect, EditorWindow parent) 
    {
        _cbSelect = cbSelect;
        _parentWin = parent;
    }

    private void onItemChosen(object obj)
    {
        var data = obj as EventBaseData;
        _cbSelect?.Invoke(data);
        Close();
    }

    private void BindItem(VisualElement element, int index)
    {
        var btn = element as Label;
        btn.text = lstDatas[index].desc;
    }

    private VisualElement ItemCreator()
    {
        return new Label();
    }
}