using UnityEngine;
using System.Collections;

public enum EUILayer
{
    Auto,
    Top
}

public enum EUIState
{
    Showing,
    Hided
}

[RequireComponent(typeof(CanvasGroup))]
public class UIBase : MonoBehaviour
{
    public EUILayer layer;
    CanvasGroup canvasGroup;

    RectTransform _rectTransform;

    public virtual void Init() { }
    public virtual void OnShow() 
    {
        //设置层级
        if (layer == EUILayer.Top)
        {
            //最上层
            transform.SetAsLastSibling();
        }
        State = EUIState.Showing;
    }
    public virtual void OnHide() 
    {
        State = EUIState.Hided;
    }

    protected virtual void OnAwake() { }
    protected virtual void OnUpdate() { }

    public virtual void OnDestroy() { }

    bool interactable = false;
    bool blockRay = false;

    public EUIState State { get; set; }

    protected void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        interactable = canvasGroup.interactable;
        blockRay = canvasGroup.blocksRaycasts;
        OnAwake();
    }

    private void Update()
    {
        if (State == EUIState.Showing)
        {
            OnUpdate();
        }
    }

    public void SetVisible(bool visible)
    {
        canvasGroup.alpha = visible ? 1 : 0;
        canvasGroup.interactable = visible && interactable;
        canvasGroup.blocksRaycasts = visible && blockRay;
    }

    public RectTransform GetRectTransform()
    {
        if (_rectTransform == null)
        {
            _rectTransform = GetComponent<RectTransform>();
        }
        return _rectTransform;
    }
}
