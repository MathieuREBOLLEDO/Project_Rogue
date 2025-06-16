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
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState CurrentState;// { get; private set; }

    public UnityEvent OnGameStateChanged;

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

    public bool IsPlaying() => CurrentState == GameState.Playing;
    public bool IsWaiting() => CurrentState == GameState.WaitingForBalls;
}

