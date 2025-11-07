using MoreMountains.Tools;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }
    public List<ItemSO> activeItems = new List<ItemSO>();

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

        // Initialise les effets (abonnements aux GameEvents, timers, etc.)
        foreach (var itemeffect in item.effects)
        {
            itemeffect.effect.Initialize();
           //itemeffect.effect.Apply(itemeffect.parameters.); // ex : abonne l'effet aux GameEvents appropriés
        }
    }

    public void RemoveItem(ItemSO item)
    {
        if (activeItems.Contains(item))
            activeItems.Remove(item);
    }

    public void OnGameEvent(GameEvent gameEvent, GameContext context)
    {
        foreach (var item in activeItems)
        {
            foreach (var effectData in item.effects)
            {
                // Convertit la liste de paramètres en dictionnaire
                Dictionary<ValueKey, string> paramDict = new Dictionary<ValueKey, string>();
                foreach (var param in effectData.parameters)
                {
                    paramDict[param.key] = param.value;
                }

                // Appelle l'effet
                effectData.effect.Initialize();
                //seffectData.effect.Apply(context, paramDict);
            }
        }
    }
}
