using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "ScriptableBrick Data" , menuName = "Bricks/Scriptable Brick")]
public class ScriptableBricks : ScriptableObject
{
    [SerializeField] private Color brickColor = Color.white;
    [SerializeField] private int life = 1;
    [SerializeField] private int bonusPointWhenDestroy = 100;

}
