using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AddNumberOfShootEffect", menuName = "Game/Effects/NumberOFShoot")]
public class AddNumberOfShoot : EffectSO
{
    [SerializeField] private int amountToAdd = 1;
    private bool hasBeenUsed = false;

    public override void Initialize()
    {
        if (hasBeenUsed)
        {
            Debug.Log($"[Effect] {effectName} déjà utilisé, ignoré.");
            return;
        }

        if (ShootHandler.Instance == null)
        {
            Debug.LogError("[Effect] Aucun ShootHandler trouvé dans la scène !");
            return;
        }

        ShootHandler.Instance.AddShootNumber(amountToAdd);
        Debug.Log($"[Effect] +{amountToAdd} shoot ajouté via {effectName}");

        hasBeenUsed = true;
    }

    public override void Cleanup()
    {
        // Rien à nettoyer car l’effet est one-shot et ne s’abonne à rien
    }
}
