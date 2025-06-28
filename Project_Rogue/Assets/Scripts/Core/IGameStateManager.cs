using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameStateManager
{
    GameState CurrentState { get; }
    void SetState(GameState newState);
}

