using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private IGameStateManager _gameStateManager;
    private ITurnState _playerTurn;
    private ITurnState _machineTurn;
    private ITurnState _currentTurn;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _gameStateManager = new GameStateManager();
        _playerTurn = new PlayerTurnState(_gameStateManager);
        _machineTurn = new MachineTurnState(_gameStateManager, this);
    }

    public void StartPlayerTurn()
    {
        _currentTurn?.Exit();
        _currentTurn = _playerTurn;
        _currentTurn.Enter();
    }

    public void StartMachineTurn()
    {
        _currentTurn?.Exit();
        _currentTurn = _machineTurn;
        _currentTurn.Enter();
    }
}


