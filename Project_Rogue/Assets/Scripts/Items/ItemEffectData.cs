using System;
using System.Collections.Generic;
using UnityEngine;

public enum ValueKey
{
    Unknown,
    Amount,
    Duration,
    Percentage
}


[Serializable]
public class ItemEffectData
{
    public EffectSO effect; // Référence vers le ScriptableObject d'effet
    public List<EffectParameter> parameters; // Paramètres spécifiques
}

[Serializable]
public class EffectParameter
{
    public GameEventType eventType; 
    public ValueKey key;  // ex: "amount", "duration"
    public string value; // sérialisé sous forme de string pour flexibilité
}
