using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestEffect", menuName = "Game/Effects/Test")]
public class ExempleEffectSO: EffectSO
{
    public override void Apply(GameContext context, Dictionary<string, string> parameters)
    {
        if (parameters.TryGetValue("amount", out string value))
        {
            if (float.TryParse(value, out float healAmount))
            {
                //context.player.Heal(healAmount);
            }
        }
    }

  //  public override void Initialize()
  //  {
  //      throw new System.NotImplementedException();
  //  }
  //
  //  public override void Clenaup()
  //  {
  //      throw new System.NotImplementedException();
  //  }
}
