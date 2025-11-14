using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }
    public List<ItemSO> activeItems = new List<ItemSO>();

    // Liste d'effets runtime actifs (lié aux items)
    private List<EffectRuntime> activeEffectRuntimes = new List<EffectRuntime>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddItem(ItemSO item)
    {
        if (item == null)
        {
            Debug.LogWarning("Tentative d'ajouter un item nul à la liste des items actifs.");
            return;
        }

        if (activeItems.Contains(item))
        {
            Debug.Log($"L'item {item.name} est déjà actif et ne peut pas être empilé.");
            return;
        }

        activeItems.Add(item);
        Debug.Log($"Item ajouté : {item.name}");

        // Initialise les effets (crée des runtime, initialise)
        foreach (var itemEffect in item.effects)
        {
            if (itemEffect == null || itemEffect.effect == null)
            {
                Debug.LogWarning($"Item {item.name} a un effet null.");
                continue;
            }

            // Construire le dictionnaire de paramètres
            Dictionary<ValueKey, string> paramDict = new Dictionary<ValueKey, string>();
            foreach (var p in itemEffect.parameters)
            {
                paramDict[p.key] = p.value;
            }

            // Créer runtime
            EffectRuntime runtime = new EffectRuntime
            {
                ownerItem = item,
                effectData = itemEffect,
                hasBeenUsed = false
            };

            activeEffectRuntimes.Add(runtime);

            // Appel Initialize du SO (pour abonner si besoin)
            itemEffect.effect.Initialize(runtime, paramDict);

            // Si trigger OnAdd ou OnEquip immédiat -> exécuter tout de suite
            if (itemEffect.trigger == EffectTrigger.OnAdd || itemEffect.trigger == EffectTrigger.OnEquip)
            {
                itemEffect.effect.Execute(GameEventType.OnItemEquiped, null, runtime, paramDict);
            }
        }
    }

    public void RemoveItem(ItemSO item)
    {
        if (!activeItems.Contains(item))
            return;

        // Cleanup des runtime liés à cet item
        for (int i = activeEffectRuntimes.Count - 1; i >= 0; i--)
        {
            var r = activeEffectRuntimes[i];
            if (r.ownerItem == item)
            {
                // build params dict for cleanup (optional)
                Dictionary<ValueKey, string> paramDict = new Dictionary<ValueKey, string>();
                foreach (var p in r.effectData.parameters)
                    paramDict[p.key] = p.value;

                r.effectData.effect.Cleanup(r);
                activeEffectRuntimes.RemoveAt(i);
            }
        }

        activeItems.Remove(item);
    }

    // Appelé par le GameEvent system de ton jeu quand un event arrive
    public void OnGameEvent(GameEventType gameEvent, GameContext context)
    {
        // Parcourir les runtimes et exécuter ceux dont le trigger correspond
        foreach (var runtime in activeEffectRuntimes)
        {
            var itemEffect = runtime.effectData;
            if (itemEffect == null || itemEffect.effect == null) continue;

            // reconstruire le dict de params (on peut le cache si on veut optimiser)
            Dictionary<ValueKey, string> paramDict = new Dictionary<ValueKey, string>();
            foreach (var p in itemEffect.parameters)
                paramDict[p.key] = p.value;

            // Décider si on doit exécuter selon le trigger
            bool shouldExecute = false;
            switch (itemEffect.trigger)
            {
                case EffectTrigger.OnPickup:
                    shouldExecute = gameEvent == GameEventType.OnItemPicked;
                    break;
                case EffectTrigger.OnDestroy:
                    shouldExecute = gameEvent == GameEventType.OnItemDestroyed;
                    break;
                case EffectTrigger.OnUse:
                    shouldExecute = gameEvent == GameEventType.OnItemUSed;
                    break;
                case EffectTrigger.OnGameEvent:
                    // Exécute pour tout GameEvent (ou tu peux étendre ItemEffectData pour filtrer précisément)
                    shouldExecute = true;
                    break;
                    // autres cas gérés dans AddItem (OnAdd/OnEquip)
            }

            if (shouldExecute)
            {
                itemEffect.effect.Execute(gameEvent, context, runtime, paramDict);
            }
        }
    }
}
