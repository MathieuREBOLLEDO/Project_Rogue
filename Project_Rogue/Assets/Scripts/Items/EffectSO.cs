using UnityEngine;
using System.Collections.Generic;

public abstract class EffectSO : ScriptableObject
{
    public string effectName;

   public abstract void Initialize();
   public abstract void Cleanup();

    /// <summary>
    /// Réinitialise l’état de l’effet (appelé au début d’une partie)
    /// </summary>
    public virtual void ResetEffectState() { }
}
