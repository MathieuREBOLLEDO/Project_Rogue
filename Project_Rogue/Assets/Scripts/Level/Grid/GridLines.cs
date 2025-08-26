using UnityEngine;
using UnityEngine.UI;

public class GridLines : MonoBehaviour
{
    [SerializeField] ScriptableBrickData brickData;
    //public int rows = 6;
    //public int cols = 4;
    public RectTransform gridParent;
    public Color lineColor = Color.green;
    public float lineThickness = 2f;

    private float cellSize;

    private void Start()
    {
        DrawGrid();
    }
    public void DrawGrid()
    {
        int cols = brickData.columns;
        int rows = brickData.rows;

        // Supprime les anciennes lignes
        foreach (Transform child in gridParent)
        {
            if (child.name.StartsWith("Line"))
                Destroy(child.gameObject);
        }

        //float screenWidth = Screen.width;
        //float screenHeight = Screen.height;

        Vector2 screenMin = ScreenUtils.ScreenMin;
        Vector2 screenMax = ScreenUtils.ScreenMax;
        float screenWidth = screenMax.x - screenMin.x;
        float screenHeight = screenMax.y - screenMin.y;
        //
        //float cellWidth = screenWidth / cols;
        //float cellHeight = screenHeight / rows;
        cellSize = brickData.cellSize;

        float totalGridWidth = cellSize * rows;
        float totalGridHeight = cellSize * cols;
        
        //Vector2 startOffset = new Vector2 (0,0);

        Vector2 startOffset = new Vector2(
            (screenWidth - totalGridWidth) / 2f,
            (screenHeight - totalGridHeight) / 2f
        );

        // Lignes verticales
        for (int i = 0; i <= cols; i++)
        {
            CreateLine(
                new Vector2(startOffset.x + i * cellSize, startOffset.y),
                new Vector2(startOffset.x + i * cellSize, startOffset.y + totalGridHeight)
            );
        }

        // Lignes horizontales
        for (int j = 0; j <= rows; j ++)
        {
            CreateLine(
                new Vector2(startOffset.x, startOffset.y + j * cellSize),
                new Vector2(startOffset.x + totalGridWidth, startOffset.y + j * cellSize)
            );
        }
    }

    void CreateLine(Vector2 start, Vector2 end)
    {
        //Debug.LogError("Start : " + start);
        //Debug.LogError("End : " + end);
        GameObject line = new GameObject("Line", typeof(Image));
        line.transform.SetParent(gridParent, false);

        RectTransform rt = line.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(0, 0);
        rt.pivot = new Vector2(0, 0);

        float length = Vector2.Distance(start, end);
        rt.sizeDelta = new Vector2(lineThickness, length);

        rt.anchoredPosition = start;
        rt.rotation = Quaternion.FromToRotation(Vector3.up, (end - start).normalized);

        Image img = line.GetComponent<Image>();
        img.color = lineColor;
    }
}
