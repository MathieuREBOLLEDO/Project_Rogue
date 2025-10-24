using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AddNumberOfShootEffect", menuName = "Game/Effects/NumberOFShoot")]
public class AddNumberOfShoot : EffectSO
{
    public override void Apply(GameContext context, Dictionary<string, string> parameters)
    {
        if (parameters.TryGetValue("amount", out string value))
        {
            if (int.TryParse(value, out int shootAmount))
            {
                context.shootHandler.AddShootNumber(shootAmount);
                //AddShootNumber(shootAmount);
            }
        }
    }
}
