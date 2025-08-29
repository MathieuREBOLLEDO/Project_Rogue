using UnityEngine;
using UnityEngine.Events;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);        
    }

    public void StartPlayerTurn()
    {
        GameEventManager.Instance.TriggerEvent(new GameEvent(GameEventType.PlayerTurnStart));
    }

    public void NotifyPlayerTurnEnd()
    {
        GameEventManager.Instance.TriggerEvent(new GameEvent(GameEventType.PlayerTurnEnd));
        StartBallTurn();
    }

    public void StartBallTurn()
    {
        GameEventManager.Instance.TriggerEvent(new GameEvent(GameEventType.BallsTurnStart));
    }
    public void NotifyBallTurnEnd()
    {
        GameEventManager.Instance.TriggerEvent(new GameEvent(GameEventType.BallsTurnEnd));
        StartMachineTurn();
    }

    public void StartMachineTurn()
    {
        GameEventManager.Instance.TriggerEvent(new GameEvent(GameEventType.MachineTurnStart));
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
