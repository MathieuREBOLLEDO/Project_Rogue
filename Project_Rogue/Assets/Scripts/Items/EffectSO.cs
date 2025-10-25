using UnityEngine;
using System.Collections.Generic;

public abstract class EffectSO : ScriptableObject
{
    public string effectName;

    // M�thode abstraite que chaque effet doit impl�menter
    public abstract void Apply(GameContext context, Dictionary<string, string> parameters);
   
    ///A ne pas utiliser, implique beaucoup de redondance. 
  /// N�cessite de savoir quelle genre d'item et ou l'utilise t'on
    // public abstract void Initialize();
   // public abstract void Clenaup();
}
