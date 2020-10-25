using UnityEngine;
using System.Collections;

public abstract class GameStateBase : IGameState
{
    public EGameState stateKey;
    [HideInInspector]
    public bool hasInit = false;
    public abstract void Init();
    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void OnUpdate();
}
