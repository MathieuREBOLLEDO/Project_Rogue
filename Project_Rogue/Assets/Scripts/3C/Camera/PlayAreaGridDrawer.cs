using UnityEngine;

public class PlayAreaGridDrawer : MonoBehaviour
{
    public PlayAreaLayoutBuilder builder;
    public Color gridColor = Color.green;
    public float lineThickness = 1f;

    private void OnDrawGizmos()
    {
        if (builder == null) return;
        if (!builder.Build()) return;

        var layout = builder.layout;
        if (layout == null || layout.globalGrid == null) return;

        Rect area = PlayAreaProvider.Instance.PlayArea;

        Gizmos.color = gridColor;

        float cellW = builder.CellWidth;
        float cellH = builder.CellHeight;

        // Vertical lines
        for (int i = 0; i <= layout.globalGrid.columns; i++)
        {
            float x = area.xMin + i * cellW;
            DrawThickLine(
                new Vector3(x, area.yMin),
                new Vector3(x, area.yMax),
                lineThickness
            );
        }

        // Horizontal lines
        for (int j = 0; j <= layout.globalGrid.rows; j++)
        {
            float y = area.yMin + j * cellH;
            DrawThickLine(
                new Vector3(area.xMin, y),
                new Vector3(area.xMax, y),
                lineThickness
            );
        }
    }

    // Gizmos n'ont pas d'épaisseur -> on fake
    void DrawThickLine(Vector3 a, Vector3 b, float thickness)
    {
        Vector3 dir = (b - a).normalized;
        Vector3 normal = Vector3.Cross(dir, Vector3.forward) * thickness * 0.01f;

        Gizmos.DrawLine(a + normal, b + normal);
        Gizmos.DrawLine(a - normal, b - normal);
    }
}
