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
    public EffectSO effect; // R�f�rence vers le ScriptableObject d'effet
    public List<EffectParameter> parameters; // Param�tres sp�cifiques
}

[Serializable]
public class EffectParameter
{
    public GameEventType eventType; 
    public ValueKey key;  // ex: "amount", "duration"
    public string value; // s�rialis� sous forme de string pour flexibilit�
}
