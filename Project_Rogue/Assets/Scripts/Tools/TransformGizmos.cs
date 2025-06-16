using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TransformGizmos : MonoBehaviour
{
    public float gizmoLength = 1.0f;
    public bool showLabels = true;

    void OnDrawGizmos()
    {
        Transform t = transform;

        // Position point
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(t.position, 0.05f);

        // Forward direction (Z)
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(t.position, t.position + t.forward * gizmoLength);
        if (showLabels)
            DrawLabel(t.position + t.forward * gizmoLength, "Forward (Z)");

        // Right direction (X)
        Gizmos.color = Color.red;
        Gizmos.DrawLine(t.position, t.position + t.right * gizmoLength);
        if (showLabels)
            DrawLabel(t.position + t.right * gizmoLength, "Right (X)");

        // Up direction (Y)
        Gizmos.color = Color.green;
        Gizmos.DrawLine(t.position, t.position + t.up * gizmoLength);
        if (showLabels)
            DrawLabel(t.position + t.up * gizmoLength, "Up (Y)");
    }

    void DrawLabel(Vector3 position, string text)
    {
#if UNITY_EDITOR
        UnityEditor.Handles.Label(position, text);
#endif
    }
}

