using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Game Data/Player Data")]
public class PlayerData : ScriptableObject
{
    [Header("Stats de Base")]
    public int maxHealth = 100;
    public int startingMoney = 0;
    public float moveSpeed = 5f;

    [Header("Autres stats")]
    public int strength = 10;
    public int agility = 8;
    public int intelligence = 6;
}
