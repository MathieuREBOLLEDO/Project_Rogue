using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
public enum GameState
{
    Menu,
    Playing,
    WaitingForBalls,
    Paused,
    GameOver
}
public enum TurnState
{
    PlayerTurn,
    MachineTurn,
    Idle
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState CurrentState;// { get; private set; }

    public UnityEvent OnGameStateChanged;

    public TurnState CurrentTurnState;// { get; private set; } = TurnState.Idle;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetState(GameState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;
        OnGameStateChanged?.Invoke();
        Debug.Log("GameState changed to: " + newState);   
    }

    public void SetTurnState(TurnState newTurnState)
    {
        CurrentTurnState = newTurnState;
        Debug.Log("TurnState changed to: " + newTurnState);
    }


    public void StartPlayerTurn()
    {
        SetTurnState(TurnState.PlayerTurn);
        SetState(GameState.Playing);
        // Tu peux activer ici un indicateur visuel, une flèche, etc.
    }

    public void EndPlayerTurn()
    {
        SetTurnState(TurnState.MachineTurn);
        SetState(GameState.WaitingForBalls);
    }

    public void StartMachineTurn()
    {
        SetTurnState(TurnState.MachineTurn);

        // Ex : faire descendre les lignes, générer une nouvelle ligne
        StartCoroutine(HandleMachineTurn());
    }

    private IEnumerator HandleMachineTurn()
    {
        yield return new WaitForSeconds(0.1f);

        StartPlayerTurn(); // relance le tour du joueur
    }




    public bool IsPlaying() => CurrentState == GameState.Playing;
    public bool IsWaiting() => CurrentState == GameState.WaitingForBalls;
}

