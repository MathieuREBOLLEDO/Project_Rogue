using System.Collections.Generic;
using UnityEngine;

public abstract class EffectSO : ScriptableObject
{
    public string effectName;

    // appelé quand l'item est ajouté / initialisé (par ItemManager) - runtime contient l'état
    public virtual void Initialize(EffectRuntime runtime, Dictionary<ValueKey, string> parameters) { }

    // Le coeur : appliquer/execute l'effet — appelé quand le trigger correspond
    public abstract void Apply(GameContext context, Dictionary<ValueKey, string> parameters);

    // Cleanup possible à la suppression de l'item
    public virtual void Cleanup(EffectRuntime runtime) { }

    // méthode utilitaire pour récupérer un param typé
    protected static T GetParam<T>(Dictionary<ValueKey, string> p, ValueKey key, T defaultValue = default)
    {
        if (p != null && p.TryGetValue(key, out var raw))
        {
            try
            {
                if (typeof(T) == typeof(int))
                    return (T)(object)int.Parse(raw);
                if (typeof(T) == typeof(float))
                    return (T)(object)float.Parse(raw);
                if (typeof(T) == typeof(string))
                    return (T)(object)raw;
                if (typeof(T) == typeof(bool))
                    return (T)(object)bool.Parse(raw);
            }
            catch
            {
                Debug.LogWarning($"Param parsing failed for key {key} with value '{raw}' to type {typeof(T)}");
            }
        }
        return defaultValue;
    }
}
