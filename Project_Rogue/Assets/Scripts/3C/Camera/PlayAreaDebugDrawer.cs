using UnityEngine;

public class PlayAreaDebugDrawer : MonoBehaviour
{
    public PlayAreaLayoutBuilder builder;

    private void OnDrawGizmos()
    {
        if (builder == null) return;
        if (!builder.Build()) return;

        foreach (var kvp in builder.Zones)
        {
            ZoneData data = kvp.Value;
            Rect r = data.rect;

            Gizmos.color = data.color;
            DrawRect(r);
        }
    }

    void DrawRect(Rect r)
    {
        Gizmos.DrawLine(new Vector3(r.xMin, r.yMin), new Vector3(r.xMax, r.yMin));
        Gizmos.DrawLine(new Vector3(r.xMax, r.yMin), new Vector3(r.xMax, r.yMax));
        Gizmos.DrawLine(new Vector3(r.xMax, r.yMax), new Vector3(r.xMin, r.yMax));
        Gizmos.DrawLine(new Vector3(r.xMin, r.yMax), new Vector3(r.xMin, r.yMin));
    }
}
