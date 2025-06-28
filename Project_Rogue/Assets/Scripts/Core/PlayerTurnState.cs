using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnState : ITurnState
{
    private readonly IGameStateManager _gameStateManager;
    public PlayerTurnState(IGameStateManager gameStateManager)
    {
        _gameStateManager = gameStateManager;
    }

    public void Enter()
    {
        _gameStateManager.SetState(GameState.Playing);
        // Indicateurs visuels, etc.
    }

    public void Exit()
    {
        Debug.Log("Player turn ended.");
    }
}

