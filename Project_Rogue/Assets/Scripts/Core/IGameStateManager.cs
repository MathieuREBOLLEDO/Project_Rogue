public interface IGameStateManager
{
    GameState CurrentState { get; }
    void SetState(GameState newState);
}

