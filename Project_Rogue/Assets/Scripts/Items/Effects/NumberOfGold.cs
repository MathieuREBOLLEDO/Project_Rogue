using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NumberOfGolds",menuName = "")]
public class NumberOfGold : EffectSO
{
    public int defaultAmountToAdd = 1;
    public override void Initialize(EffectRuntime runtime, Dictionary<ValueKey, string> parameters) { }

    public override void Apply(GameContext context, Dictionary<ValueKey, string> parameters)
    {
        int amount = GetParam<int>(parameters, ValueKey.Amount, defaultAmountToAdd);

        if (ShootHandler.Instance == null)
        {
            Debug.LogError("[Effect] Aucun ShootHandler trouvé dans la scène !");
            return;
        }

        ShootHandler.Instance.AddShootNumber(amount);
        Debug.Log(" L'effet à était appelé");
        //Debug.Log($"[Effect] +{amount} shoot ajouté via {effectName} pour {runtime.ownerItem?.itemName}");

        //runtime.hasBeenUsed = true;
    }

    public override void Cleanup(EffectRuntime runtime) {  }
}
