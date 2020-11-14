using System;
using System.Collections.Generic;
using System.Text;
using SimpleJSON;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class UIGetItem : UIBase
{
    public Text text;
    public Button btnClose;

    List<ItemData> items;

    Transform tfRoot;

    public override void Init()
    {
        base.Init();
        btnClose.onClick.AddListener(OnBtnClose);
        tfRoot = GameUtil.FindChild(gameObject, "Root").transform;
    }

    private void OnBtnClose()
    {
        UI.UIMgr.Inst.HideUI(UITable.EUITable.UIGetItem);
    }

    public void SetData(List<ItemData> items)
    {
        this.items = items;
    }

    public void Refresh()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var item in items)
        {
            sb.AppendLine(item.baseData.name + "X" + item.count);
        }
        text.text = sb.ToString();
        tfRoot.DOScale(Vector3.one, 0.2f).From(new Vector3(1, 0.1f, 1f));
    }
}