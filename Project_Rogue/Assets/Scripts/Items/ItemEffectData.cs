using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemEffectData
{
    public EffectSO effect; // Référence vers le ScriptableObject d'effet
    public List<EffectParameter> parameters; // Paramètres spécifiques
}

[Serializable]
public class EffectParameter
{
    public string key;  // ex: "amount", "duration"
    public string value; // sérialisé sous forme de string pour flexibilité
}
