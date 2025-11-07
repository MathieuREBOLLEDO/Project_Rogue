using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private List<EffectSO> allEffects; // à remplir dans l’inspecteur, ou via un registry global

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);       
        StartNewGame();
    }


    public void StartNewGame()
    {
        ResetAllEffects();
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

    private void ResetAllEffects()
    {
#if UNITY_EDITOR
        string[] guids = UnityEditor.AssetDatabase.FindAssets("t:EffectSO");
        foreach (string guid in guids)
        {
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
            EffectSO effect = UnityEditor.AssetDatabase.LoadAssetAtPath<EffectSO>(path);
            effect.ResetEffectState();
        }
#else
    // En build, il faudra gérer autrement, ex. via une liste globale statique
#endif
    }
}

