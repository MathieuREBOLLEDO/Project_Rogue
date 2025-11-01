using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestEffect", menuName = "Game/Effects/Test")]
public class ExempleEffectSO: EffectSO
{
    public override void Apply(GameContext context, Dictionary<ValueKey, string> parameters)
    {
        if (parameters.TryGetValue(ValueKey.Amount, out string value))
        {
            if (float.TryParse(value, out float healAmount))
            {
                //context.player.Heal(healAmount);
            }
        }
    }
}
