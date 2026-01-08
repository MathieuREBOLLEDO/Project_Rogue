using UnityEngine;

public class PlayAreaGridDrawer : MonoBehaviour
{
    [Header("Grid settings")]
    public int columns = 8;
    public int rows = 6;

    public Color lineColor = Color.green;
    public float lineWidth = 0.05f;

    [Header("Debug")]
    public bool drawInGame = true;   // LineRenderer
    public bool drawInEditor = true; // Gizmos

    private void Start()
    {
        if (drawInGame)
            DrawGridRuntime();
    }

    #region Runtime drawing (LineRenderer)

    void DrawGridRuntime()
    {
        Rect area = PlayAreaProvider.Instance.PlayArea;

        // Vertical lines
        for (int i = 0; i <= columns; i++)
        {
            float t = (float)i / columns;
            float x = Mathf.Lerp(area.xMin, area.xMax, t);

            CreateLine(
                new Vector3(x, area.yMin, 0),
                new Vector3(x, area.yMax, 0)
            );
        }

        // Horizontal lines
        for (int j = 0; j <= rows; j++)
        {
            float t = (float)j / rows;
            float y = Mathf.Lerp(area.yMin, area.yMax, t);

            CreateLine(
                new Vector3(area.xMin, y, 0),
                new Vector3(area.xMax, y, 0)
            );
        }
    }

    void CreateLine(Vector3 start, Vector3 end)
    {
        GameObject go = new GameObject("GridLine");
        go.transform.SetParent(transform, false);

        LineRenderer lr = go.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = lineColor;
        lr.endColor = lineColor;
        lr.sortingOrder = 1;
    }

    #endregion

    #region Editor drawing (Gizmos)

    private void OnDrawGizmos()
    {
        if (!drawInEditor) return;
        if (PlayAreaProvider.Instance == null) return;

        Rect area = PlayAreaProvider.Instance.PlayArea;

        Gizmos.color = lineColor;

        // Vertical
        for (int i = 0; i <= columns; i++)
        {
            float t = (float)i / columns;
            float x = Mathf.Lerp(area.xMin, area.xMax, t);

            Gizmos.DrawLine(
                new Vector3(x, area.yMin, 0),
                new Vector3(x, area.yMax, 0)
            );
        }

        // Horizontal
        for (int j = 0; j <= rows; j++)
        {
            float t = (float)j / rows;
            float y = Mathf.Lerp(area.yMin, area.yMax, t);

            Gizmos.DrawLine(
                new Vector3(area.xMin, y, 0),
                new Vector3(area.xMax, y, 0)
            );
        }
    }

    #endregion
}
