using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestEffect", menuName = "Game/Effects/Test")]
public class ExempleEffectSO: EffectSO
{
    public override void Initialize(EffectRuntime runtime, Dictionary<ValueKey, string> parameters)
    {
        Debug.Log("Test effect sucessefully Initliazed");
    }

    public override void Execute(GameEventType gameEvent, GameContext context, EffectRuntime runtime, Dictionary<ValueKey, string> parameters)
    {
        Debug.Log("Test effect sucessefully remove");
    }

    public override void Cleanup(EffectRuntime runtime)
    {

        Debug.Log("Test effect sucessefully Cleanup");
    }
}
