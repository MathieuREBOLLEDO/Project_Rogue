using UnityEngine;
using UnityEngine.Events;
public enum GameState
{
    Menu,
    Playing,
    WaitingForBalls,
    AllBallOnGround,
    MachineTurn,
    Paused,
    GameOver
}

public class GameStateManager : IGameStateManager
{
    public GameState CurrentState { get; private set; }
    public UnityEvent OnGameStateChanged = new UnityEvent();

    public void SetState(GameState newState)
    {
        if (CurrentState == newState) return;
        CurrentState = newState;
        EventBus.PublishGameStateChange(newState);
        Debug.Log($"GameState changed to: {newState}");
    }

    public bool IsPlaying() => CurrentState == GameState.Playing;
    public bool IsWaiting() => CurrentState == GameState.WaitingForBalls;
}