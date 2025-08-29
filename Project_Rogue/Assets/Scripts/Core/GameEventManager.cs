using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager Instance { get; private set; }

    private Dictionary<GameEventType, Action<GameEvent>> eventTable = new();

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

    public void Subscribe(GameEventType eventType, Action<GameEvent> callback)
    {
        if (!eventTable.ContainsKey(eventType))
            eventTable[eventType] = delegate { };

        eventTable[eventType] += callback;
    }

    public void Unsubscribe(GameEventType eventType, Action<GameEvent> callback)
    {
        if (eventTable.ContainsKey(eventType))
            eventTable[eventType] -= callback;
    }

    public void TriggerEvent(GameEvent gameEvent)
    {
        if (Instance.eventTable.ContainsKey(gameEvent.type))
            Instance.eventTable[gameEvent.type]?.Invoke(gameEvent);
    }
}
