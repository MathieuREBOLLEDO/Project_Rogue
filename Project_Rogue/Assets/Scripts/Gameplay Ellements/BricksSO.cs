using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "NewBrickData" , menuName = "Game/Brick")]
public class BricksSO : ScriptableObject
{
    [SerializeField] private Color brickColor = Color.white;
    [SerializeField] private int life = 1;
    [SerializeField] private int bonusPointWhenDestroy = 100;

}
