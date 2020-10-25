using System;
using System.Collections.Generic;
using UnityEngine;

public class UIItemLine : UIItemBase
{
    private Vector2 startPoint;
    private Vector2 endPoint;

    RectTransform rectTf;

    public void Init(Vector2 startPoint,Vector2 endPoint)
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;
    }

    public override void OnAwake()
    {
        base.OnAwake();
        rectTf = GetComponent<RectTransform>();
    }

    public override void Refresh()
    {
        base.Refresh();
        rectTf.anchoredPosition = startPoint;
        rectTf.localRotation = Quaternion.LookRotation(Vector3.forward, endPoint - startPoint);
        rectTf.sizeDelta = new Vector2(rectTf.sizeDelta.x, Vector2.Distance(endPoint, startPoint));
    }
}
