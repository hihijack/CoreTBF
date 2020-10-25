using System;
using UI;
using UnityEngine.UI;

public class UIMainStage : UIBase
{
    public Button btnStartRaid;

    public override void Init()
    {
        base.Init();
        btnStartRaid.onClick.AddListener(OnBtnStartRaid);
    }

    private void OnBtnStartRaid()
    {
        UIMgr.Inst.HideUI(UITable.EUITable.UIMainStage);
        UIMgr.Inst.ShowUI(UITable.EUITable.UIIntoRaid);
    }
}
