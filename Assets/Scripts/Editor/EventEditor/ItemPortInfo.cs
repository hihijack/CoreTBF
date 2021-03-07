using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class ItemPortInfo : Box
{
    public PortInfoData data;

    EventNodeView _nodeSrouce;
    PortsListView _portListView;
    TextField _txtDesc;
    public ItemPortInfo(EventNodeView nodeSource, PortsListView plv) 
    {
        _nodeSrouce = nodeSource;
        _portListView = plv;
        _txtDesc = new TextField();
        _txtDesc.name = "tfInput";
        _txtDesc.RegisterValueChangedCallback(OnTxtDescChanged);
        this.Add(_txtDesc);
        Button btnMoveDonw = new Button(OnClickMoveDown);
        btnMoveDonw.text = "下移";
        this.Add(btnMoveDonw);
        Button btnMoveUp = new Button(OnClickMoveUp);
        btnMoveUp.text = "上移";
        this.Add(btnMoveUp);
        Button btnDelete = new Button(OnClickDelete);
        btnDelete.text = "删除";
        this.Add(btnDelete);
        this.AddToClassList("protListBox");
    }

    private void OnTxtDescChanged(ChangeEvent<string> evt)
    {
        _nodeSrouce.SetPortInfoDataDesc(data, evt.newValue);
    }

    private void OnClickDelete()
    {
        _nodeSrouce.DeletePortInfoData(data);
        _portListView.OnDelete(this);
    }

    internal void SetData(PortInfoData portInfoData)
    {
        this.data = portInfoData;
    }

    public void Refresh()
    {
        _txtDesc.value = data.desc;
    }

    private void OnClickMoveUp()
    {
        _nodeSrouce.MovePortInfoDataUp(data);
        _portListView.SortWithIndex();
    }

    private void OnClickMoveDown()
    {
        _nodeSrouce.MovePortInfoDtatDown(data);
        _portListView.SortWithIndex();
    }
}
