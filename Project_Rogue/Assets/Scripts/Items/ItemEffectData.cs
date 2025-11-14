using System;
using System.Collections.Generic;
using UnityEngine;

public enum ValueKey
{
    Unknown,
    Amount,
    Duration,
    TargetTag,
    Percentage
}

public enum EffectTrigger
{
    OnAdd,      // exécute immédiatement quand l'item est ajouté
    OnPickup,
    OnDestroy,
    OnUse,
    OnEquip,
    OnGameEvent, // option générique, on peut filtrer plus finement par GameEvent passé
    // etc.
}

[Serializable]
public class ItemEffectData
{
    public EffectSO effect;
    public EffectTrigger trigger = EffectTrigger.OnPickup;
    public List<EffectParameter> parameters = new List<EffectParameter>();
}

[Serializable]
public class EffectParameter
{
    public GameEventType eventType; 
    public ValueKey key;  // ex: "amount", "duration"
    public string value; // sérialisé sous forme de string pour flexibilité
}

public class EffectRuntime
{
    public ItemSO ownerItem;
    public ItemEffectData effectData;
    public bool hasBeenUsed = false; // état par-instance
    // tu peux ajouter un id, timers, subscriptions, etc.
}