using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }
    public List<ItemSO> activeItems = new List<ItemSO>();

    private ItemSO currentItemPicked;

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

    private void Start()
    {
        GameEventManager.Instance.Subscribe(GameEventType.OnItemPicked, ItemPicked);
    }
    private void OnDisable()
    {
        GameEventManager.Instance.Unsubscribe(GameEventType.MachineTurnStart, ItemPicked);
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

        currentItemPicked = item;
        activeItems.Add(item);
        Debug.Log($"Item ajouté : {item.name}");

        GameEventManager.Instance.TriggerEvent(new GameEvent(GameEventType.OnItemPicked));
        
    }



    private void ItemPicked(GameEvent gameEvent)
    {
        foreach (var itemEffect in currentItemPicked.effects)
        {
            if (itemEffect == null || itemEffect.effect == null)
            {
                Debug.LogWarning($"Item {currentItemPicked.name} a un effet null.");
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
                ownerItem = currentItemPicked,
                effectData = itemEffect,
                hasBeenUsed = false
            };

            activeEffectRuntimes.Add(runtime);

            // Appel Initialize du SO (pour abonner si besoin)
            itemEffect.effect.Initialize(runtime, paramDict);

            if (itemEffect.triggerEvent == GameEventType.OnItemPicked) // itemEffect.triggerEvent == GameEventType.OnAdd)
            {
                itemEffect.effect.Apply( null, paramDict);
                Debug.Log("L'effet estt activé ");
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

    public void AddBonus(BonusItemSO bonus)
    {
        if (bonus == null || bonus.effect == null)
        {
            Debug.LogWarning("[BONUS] Bonus ou effet null");
            return;
        }

        StartCoroutine(HandleBonusLifecycle(bonus));
    }

    private IEnumerator HandleBonusLifecycle(BonusItemSO bonus)
    {
        // Build params dict
        Dictionary<ValueKey, string> paramDict = new Dictionary<ValueKey, string>();
        foreach (var p in bonus.parameters)
            paramDict[p.key] = p.value;

        // Créer un runtime comme pour les items
        EffectRuntime runtime = new EffectRuntime
        {
            ownerItem = null, // important : ce n’est PAS un item
            effectData = null,
            hasBeenUsed = false
        };

        // Initialize
        bonus.effect.Initialize(runtime, paramDict);
        bonus.effect.Apply(null, paramDict);

        Debug.Log($"[BONUS] Effet activé : {bonus.effect.effectName}");

        yield return new WaitForSeconds(bonus.duration);

        // Cleanup
        bonus.effect.Cleanup(runtime);

        Debug.Log($"[BONUS] Effet terminé : {bonus.effect.effectName}");
    }



    //// Appelé par le GameEvent system de ton jeu quand un event arrive
    //public void OnGameEvent(GameEventType gameEvent, GameContext context)
    //{
    //    // Parcourir les runtimes et exécuter ceux dont le trigger correspond
    //    foreach (var runtime in activeEffectRuntimes)
    //    {
    //        var itemEffect = runtime.effectData;
    //        if (itemEffect == null || itemEffect.effect == null) continue;
    //
    //        // reconstruire le dict de params (on peut le cache si on veut optimiser)
    //        Dictionary<ValueKey, string> paramDict = new Dictionary<ValueKey, string>();
    //        foreach (var p in itemEffect.parameters)
    //            paramDict[p.key] = p.value;
    //
    //        // Décider si on doit exécuter selon le trigger
    //        bool shouldExecute = false;
    //        switch (itemEffect.trigger)
    //        {
    //            case EffectTrigger.OnPickup:
    //                shouldExecute = gameEvent == GameEventType.OnItemPicked;
    //                break;
    //            case EffectTrigger.OnDestroy:
    //                shouldExecute = gameEvent == GameEventType.OnItemDestroyed;
    //                break;
    //            case EffectTrigger.OnUse:
    //                shouldExecute = gameEvent == GameEventType.OnItemUSed;
    //                break;
    //            case EffectTrigger.OnGameEvent:
    //                // Exécute pour tout GameEvent (ou tu peux étendre ItemEffectData pour filtrer précisément)
    //                shouldExecute = true;
    //                break;
    //                // autres cas gérés dans AddItem (OnAdd/OnEquip)
    //        }
    //
    //        if (shouldExecute)
    //        {
    //            itemEffect.effect.Execute(gameEvent, context, runtime, paramDict);
    //        }
    //    }
    //}

    public void OnGameEvent(GameEvent gameEvent, GameContext context)
    {
        foreach (var item in activeItems)
        {
            foreach (var effectData in item.effects)
            {
                // On ne déclenche QUE si le GameEvent correspond
                if (effectData.triggerEvent != gameEvent.eventType)
                    continue;

                // Convertit paramètres → dictionnaire
                Dictionary<ValueKey, string> paramDict = new Dictionary<ValueKey, string>();
                foreach (var param in effectData.parameters)
                    paramDict[param.key] = param.value;

                // Appelle l’effet
                effectData.effect.Apply(context, paramDict);
            }
        }
    }

}
