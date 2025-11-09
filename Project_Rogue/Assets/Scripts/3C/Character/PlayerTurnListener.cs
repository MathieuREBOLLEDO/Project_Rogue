using UnityEngine;

public class PlayerTurnListener : MonoBehaviour
{
    private void OnEnable()
    {
        GameEventManager.Instance.Subscribe(GameEventType.PlayerTurnStart, OnPlayerTurnStart);
    }

    private void OnDisable()
    {
        GameEventManager.Instance.Unsubscribe(GameEventType.PlayerTurnStart, OnPlayerTurnStart);
    }

    private void OnPlayerTurnStart(GameEvent gameEvent)
    {
        Debug.Log(" Le tour du joueur commence !");
        // Ici tu peux lancer ton code : UI, réinitialiser une arme, etc.
    }
}
