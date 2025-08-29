using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemEffectData
{
    public EffectSO effect; // R�f�rence vers le ScriptableObject d'effet
    public List<EffectParameter> parameters; // Param�tres sp�cifiques
}

[Serializable]
public class EffectParameter
{
    public string key;  // ex: "amount", "duration"
    public string value; // s�rialis� sous forme de string pour flexibilit�
}
