using MoreMountains.Tools;
using System.Collections.Generic;
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
        if (!activeItems.Contains(item))
            activeItems.Add(item);
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
                Dictionary<string, string> paramDict = new Dictionary<string, string>();
                foreach (var param in effectData.parameters)
                {
                    paramDict[param.key] = param.value;
                }

                // Appelle l'effet
                effectData.effect.Apply(context, paramDict);
            }
        }
    }
}
