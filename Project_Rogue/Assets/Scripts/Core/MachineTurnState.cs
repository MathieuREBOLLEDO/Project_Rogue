using System.Collections;
using UnityEngine;
public class MachineTurnState : ITurnState
{
    private readonly IGameStateManager _gameStateManager;
    private readonly MonoBehaviour _coroutineRunner;

    public MachineTurnState(IGameStateManager gameStateManager, MonoBehaviour coroutineRunner)
    {
        _gameStateManager = gameStateManager;
        _coroutineRunner = coroutineRunner;
    }

    public void Enter()
    {
        _gameStateManager.SetState(GameState.MachineTurn);
        _coroutineRunner.StartCoroutine(HandleMachineTurn());
    }

    public void Exit()
    {
        Debug.LogWarning("Machine turn ended.");
    }

    private IEnumerator HandleMachineTurn()
    {

        yield return new WaitForSeconds(0.1f);

        GameManager.Instance.StartPlayerTurn(); // temporaire, on peut l’extraire aussi
    }
}