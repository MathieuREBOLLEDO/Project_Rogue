using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewBonus", menuName = "Game/Bonus")]
public class BonusItemSO : ScriptableObject
{
    public string bonusName;
    public float duration = 5f;

    public EffectSO effect;

    public List<EffectParameter> parameters = new List<EffectParameter>();
}
