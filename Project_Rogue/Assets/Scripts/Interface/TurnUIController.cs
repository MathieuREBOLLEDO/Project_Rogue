using UnityEngine;

public class TurnUIController : MonoBehaviour
{
    private void OnEnable()
    {
        GameEventManager.Instance.Subscribe(GameEventType.PlayerTurnStart, OnPlayerTurnStart);
        GameEventManager.Instance.Subscribe(GameEventType.MachineTurnStart, OnMachineTurnStart);
    }

    private void OnDisable()
    {
        GameEventManager.Instance.Unsubscribe(GameEventType.PlayerTurnStart, OnPlayerTurnStart);
        GameEventManager.Instance.Unsubscribe(GameEventType.MachineTurnStart, OnMachineTurnStart);
    }

    private void OnPlayerTurnStart(GameEvent gameEvent)
    {
        // Met à jour l’UI pour indiquer le tour du joueur
    }

    private void OnMachineTurnStart(GameEvent gameEvent)
    {
        // Met à jour l’UI pour indiquer le tour de la machine
    }
}
