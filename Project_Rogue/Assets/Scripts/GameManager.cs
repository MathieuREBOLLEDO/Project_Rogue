using UnityEngine;
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
        GameEventManager.Instance.TriggerEvent(new GameEvent(GameEventType.PlayerTurnStart));
        _currentTurn.Enter();
    }

    public void NotifyPlayerTurnEnd()
    {
        GameEventManager.Instance.TriggerEvent(new GameEvent(GameEventType.PlayerTurnEnd));
        StartMachineTurn();
    }

    public void StartMachineTurn()
    {
        _currentTurn?.Exit();
        _currentTurn = _machineTurn;
        GameEventManager.Instance.TriggerEvent(new GameEvent(GameEventType.MachineTurnStart));
        _currentTurn.Enter();
    }

    public void NotifyMachineTurnEnd()
    {
        GameEventManager.Instance.TriggerEvent(new GameEvent(GameEventType.MachineTurnEnd));
        StartPlayerTurn();
    }

    public void NotifyEndGame()
    {
        GameEventManager.Instance.TriggerEvent(new GameEvent(GameEventType.GameEnd));
    }
}


/*
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public UnityEvent OnPlayerTurnStart;
    public UnityEvent OnPlayerTurnEnd;

    public UnityEvent OnBallsTurnStart;
    public UnityEvent OnBallsTurnEnd;

    public UnityEvent OnMachineTurnStart;
    public UnityEvent OnMachineTurnEnd;

    public UnityEvent OnGameEnd;
    

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
        GameEventManager.Instance.TriggerEvent(new GameEvent(GameEventType.MachineTurnStart));

        //OnMachineTurnStart?.Invoke();
        _currentTurn.Enter();
    }

    public void NotifyMachineTurnEnd()
    {
        OnMachineTurnEnd?.Invoke();
        StartPlayerTurn();
    }

    public void NotifyEndGame()
    {

    }
}

*/
