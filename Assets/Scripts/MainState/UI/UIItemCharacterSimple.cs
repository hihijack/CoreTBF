using Data;
using System;
using UnityEngine.UI;

public class UIItemCharacterSimple : UIItemBase
{
    public Image imgHeadIcon;
    public Image imgSelection;
    private Action<UIItemCharacterSimple> onClick;

    public RoleBaseData Data { get; set; }

    Button btn;

    public void Init(RoleBaseData data, Action<UIItemCharacterSimple> onClick)
    {
        this.onClick = onClick;
        this.Data = data;
        btn = imgHeadIcon.GetComponent<Button>();
        btn.onClick.AddListener(OnBtnClick);
    }

    public override void Refresh()
    {
        base.Refresh();
        imgHeadIcon.SetSprite(Data.headicon);
    }

    public override void Cache()
    {
        base.Cache();
        btn.onClick.RemoveAllListeners();
    }

    public void SetSelected(bool selected)
    {
        imgSelection.SetImageAlpha(selected ? 1f : 0f);
    }

    private void OnBtnClick()
    {
        onClick(this);
    }
}
