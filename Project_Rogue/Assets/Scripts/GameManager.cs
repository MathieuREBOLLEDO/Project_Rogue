using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public UnityEvent OnPlayerTurnStart;
    public UnityEvent OnPlayerTurnEnd;

    public UnityEvent OnBallsTurnStart;
    public UnityEvent OnBallsTurnEnd;

    public UnityEvent OnMachineTurnStart;
    public UnityEvent OnMachineTurnEnd;
    

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
        OnPlayerTurnStart?.Invoke();
        _currentTurn.Enter();
    }

    public void NotifyPlayerTurnEnd()
    {
        OnPlayerTurnEnd?.Invoke();
        StartMachineTurn();
    }


    public void StartMachineTurn()
    {
        _currentTurn?.Exit();
        _currentTurn = _machineTurn;
        OnMachineTurnStart?.Invoke();
        _currentTurn.Enter();
    }

    public void NotifyMachineTurnEnd()
    {
        OnMachineTurnEnd?.Invoke();
        StartPlayerTurn();
    }
}


