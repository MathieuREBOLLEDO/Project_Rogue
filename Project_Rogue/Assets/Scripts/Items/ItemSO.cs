using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewItem", menuName = "Game/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public List<ItemEffectData> effects;
}
