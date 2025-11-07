using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestEffect", menuName = "Game/Effects/Test")]
public class ExempleEffectSO: EffectSO
{
    public override void Initialize()
    {
        Debug.Log("Test effect sucessefully Initliazed");
    }

    public override void Cleanup()
    {
        Debug.Log("Test effect sucessefully remove");
    }
}
