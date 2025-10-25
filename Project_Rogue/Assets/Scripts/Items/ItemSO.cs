using UnityEngine;
using System.Collections.Generic;

public enum itemType
{
    Passive,
    Active,
    Bonus,
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Game/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public List<ItemEffectData> effects;
}
