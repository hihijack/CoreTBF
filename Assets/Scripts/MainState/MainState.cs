using UI;

[System.Serializable]
public class MainState : GameStateBase
{
    public override void Init()
    {
       
    }

    public override void OnEnter()
    {
        UIMgr.Inst.ShowUI(UITable.EUITable.UIMainStage);
    }

    public override void OnExit()
    {
        UIMgr.Inst.HideUI(UITable.EUITable.UIMainStage);
    }

    public override void OnUpdate()
    {
    }
}
