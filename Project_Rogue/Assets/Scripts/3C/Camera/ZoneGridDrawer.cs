using UnityEngine;

public class ZoneGridDrawer : MonoBehaviour
{
    public PlayAreaLayoutBuilder builder;
    public ZoneType zoneType;

    [Header("Debug")]
    public float borderThickness = 3f;

    private void OnDrawGizmos()
    {
        if (builder == null) return;
        if (!builder.Build()) return;
        if (!builder.Zones.TryGetValue(zoneType, out ZoneData data)) return;

        Gizmos.color = data.color;
        Rect r = data.rect;

        DrawRectThick(r, borderThickness);
    }

    void DrawRectThick(Rect r, float thickness)
    {
        DrawThickLine(new Vector3(r.xMin, r.yMin), new Vector3(r.xMax, r.yMin), thickness);
        DrawThickLine(new Vector3(r.xMax, r.yMin), new Vector3(r.xMax, r.yMax), thickness);
        DrawThickLine(new Vector3(r.xMax, r.yMax), new Vector3(r.xMin, r.yMax), thickness);
        DrawThickLine(new Vector3(r.xMin, r.yMax), new Vector3(r.xMin, r.yMin), thickness);
    }

    void DrawThickLine(Vector3 a, Vector3 b, float thickness)
    {
        Vector3 dir = (b - a).normalized;
        Vector3 normal = Vector3.Cross(dir, Vector3.forward) * thickness * 0.01f;

        Gizmos.DrawLine(a + normal, b + normal);
        Gizmos.DrawLine(a - normal, b - normal);
    }
}
