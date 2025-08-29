using UnityEngine;
using System.Collections.Generic;

public abstract class EffectSO : ScriptableObject
{
    public string effectName;

    // Méthode abstraite que chaque effet doit implémenter
    public abstract void Apply(GameContext context, Dictionary<string, string> parameters);
}
