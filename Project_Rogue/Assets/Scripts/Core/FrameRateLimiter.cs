using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateLimiter : MonoBehaviour
{
    [SerializeField, Range(10, 120)]
    private int targetFrameRate = 30; // Valeur par défaut à 30 FPS

    private void Awake()
    {
        
        QualitySettings.vSyncCount = 0; // Désactive la synchronisation verticale (vSync)
        Application.targetFrameRate = targetFrameRate;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void OnValidate()
    {
        Application.targetFrameRate = targetFrameRate;
    }

    private void Update()
    {
        if (Application.targetFrameRate != targetFrameRate)
            Application.targetFrameRate = targetFrameRate;
    }
}
