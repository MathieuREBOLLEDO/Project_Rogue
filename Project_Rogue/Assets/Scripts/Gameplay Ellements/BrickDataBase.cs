using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "NewBrickData" , menuName = "Game/Brick Datas")]
public class BrickDataBase : ScriptableObject
{
    public BrickVisual[] bricks;

    public BrickVisual GetData(BrickType type)
    {
        foreach (var b in bricks)
        {
            if (b.type == type)
                return b;
        }

        Debug.LogWarning($"No data for {type}");
        return null;
    }

    [SerializeField] private Color brickColor = Color.white;
    [SerializeField] private int life = 1;
    [SerializeField] private int bonusPointWhenDestroy = 100;

}


[System.Serializable]
public class BrickVisual
{
    public BrickType type;
    public Color color;
    public Sprite sprite;
    public int hp;
    public int bonusPointWhenDestroy; 
}
