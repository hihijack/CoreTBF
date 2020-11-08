using UI;

public enum EWorld
{
    MainWorld,
    OtherWorld
}

[System.Serializable]
public class MainState : GameStateBase
{

    public EWorld curInWorld = EWorld.MainWorld;

    public override void Init()
    {
       EventProcessor.Inst.Init();
       hasInit = true;
    }

    public override void OnEnter()
    {
        if (curInWorld == EWorld.OtherWorld)
        {
            UIMgr.Inst.ShowUI(UITable.EUITable.UIWorldTree);
            UIMgr.Inst.ShowUI(UITable.EUITable.UIWorldInfo);
           WorldRaidData.Inst.TryTriCachedOption();
        }
        else
        {
            UIMgr.Inst.ShowUI(UITable.EUITable.UIMainStage);
        }
       
    }

    public override void OnExit()
    {
        UIMgr.Inst.HideAll();
    }

    public override void OnUpdate()
    {
    }

    public void ToWorld(EWorld world)
    {
        this.curInWorld = world;
    }
}
