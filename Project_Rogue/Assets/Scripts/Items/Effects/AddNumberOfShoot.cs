using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AddNumberOfShootEffect", menuName = "Game/Effects/NumberOfShoot")]
public class AddNumberOfShoot : EffectSO
{
    // valeur par défaut si l'item ne fournit pas le paramètre
    public int defaultAmountToAdd = 1;

    public override void Initialize(EffectRuntime runtime, Dictionary<ValueKey, string> parameters)
    {
        // rien à faire par défaut, mais possible d'initialiser des timers / abonnements
    }

    public override void Execute(GameEventType gameEvent, GameContext context, EffectRuntime runtime, Dictionary<ValueKey, string> parameters)
    {
        // Exemple: si c'est un one-shot, on peut utiliser runtime.hasBeenUsed
        if (runtime.hasBeenUsed)
        {
            Debug.Log($"[Effect] {effectName} déjà utilisé pour l'item {runtime.ownerItem?.itemName}, ignoré.");
            return;
        }

        int amount = GetParam<int>(parameters, ValueKey.Amount, defaultAmountToAdd);

        if (ShootHandler.Instance == null)
        {
            Debug.LogError("[Effect] Aucun ShootHandler trouvé dans la scène !");
            return;
        }

        ShootHandler.Instance.AddShootNumber(amount);
        Debug.Log($"[Effect] +{amount} shoot ajouté via {effectName} pour {runtime.ownerItem?.itemName}");

        runtime.hasBeenUsed = true;
    }

    public override void Cleanup(EffectRuntime runtime)
    {
        // rien
    }
}
