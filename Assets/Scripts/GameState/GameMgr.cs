using UI;
using UnityEngine;



public class GameMgr : MonoBehaviour
{
    #region 单例
    private static GameMgr _inst;
    
    public static GameMgr Inst
    {
        get { return _inst; }
    }
    #endregion

    [SerializeField]
    FightState fightState;

    private void Awake()
    {
        _inst = this;
        GameData.Inst.Init();
        fightState.Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        FightState.Inst.OnEnter();
    }

    private void Update()
    {
        FightState.Inst.Update();
    }



    private void OnDestroy()
    {
        GameData.Inst.Release();
    }
}
