using System.Collections.Generic;
using UI;
using UnityEngine;

public enum EGameState
{
    MainStage, //主基地
    Fight, //战斗

}

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

    [SerializeField]
    MainState mainState;

    Dictionary<EGameState, GameStateBase> _dicStates;

    GameStateBase _curStage;

    private void Awake()
    {
        _inst = this;
        GameData.Inst.Init();
        PlayerDataMgr.Inst.ReadFromSaved();//读取存档或初始化玩家数据
        _dicStates = new Dictionary<EGameState, GameStateBase>();
        _dicStates.Add(EGameState.Fight, fightState);
        _dicStates.Add(EGameState.MainStage, mainState);
    }

    // Start is called before the first frame update
    void Start()
    {
        ToState(EGameState.MainStage);
    }

    private void Update()
    {
        if (_curStage != null)
        {
            _curStage.OnUpdate();
        }
    }

    private void OnDestroy()
    {
        GameData.Inst.Release();
    }

    public void ToState(EGameState state)
    {
        if (_curStage == null || (_curStage != null && _curStage.stateKey != state))
        {
            //状态改变
            if (_curStage != null)
            {
                _curStage.OnExit();
            }
            _curStage = _dicStates[state];
            if (!_curStage.hasInit)
            {
                _curStage.Init();
            }
            _curStage.OnEnter();
        }
    }

    public MainState MainState
    {
        get
        {
            return mainState;    
        }
    }
}
