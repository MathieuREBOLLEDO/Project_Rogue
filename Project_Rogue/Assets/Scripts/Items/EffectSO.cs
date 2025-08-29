using UnityEngine;
using System.Collections.Generic;

public abstract class EffectSO : ScriptableObject
{
    public string effectName;

    // M�thode abstraite que chaque effet doit impl�menter
    public abstract void Apply(GameContext context, Dictionary<string, string> parameters);
}
