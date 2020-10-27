using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemLine : UIItemBase
{
    private Vector2 startPoint;
    private Vector2 endPoint;

    RectTransform rectTf;

    bool enableArrive;

    Image image;

    public void Init(Vector2 startPoint,Vector2 endPoint, bool enableArrive)
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.enableArrive = enableArrive;
    }

    public override void OnAwake()
    {
        base.OnAwake();
        rectTf = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    public override void Refresh()
    {
        base.Refresh();
        rectTf.anchoredPosition = startPoint;
        rectTf.localRotation = Quaternion.LookRotation(Vector3.forward, endPoint - startPoint);
        rectTf.sizeDelta = new Vector2(rectTf.sizeDelta.x, Vector2.Distance(endPoint, startPoint));
        if (enableArrive)
        {
            image.color = Color.white;
        }else
        {
            image.color = Color.grey;
        }
    }
}
