using UnityEditor;
using UnityEngine;

public static class GlobalBallVariables
{
    public static Vector3 ballLaunchPosition;
    public static float angleOfShooting = 80;
    public static float maxAngleOfShooting = 90;

    public static float ballSize;

    public static void SetBallSize(float value)
    {
        ballSize = value;
    }
}
