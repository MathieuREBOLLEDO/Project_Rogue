using UnityEngine;
using System.Collections.Generic;

public abstract class EffectSO : ScriptableObject
{
    public string effectName;

   public abstract void Initialize();
   public abstract void Cleanup();
}
