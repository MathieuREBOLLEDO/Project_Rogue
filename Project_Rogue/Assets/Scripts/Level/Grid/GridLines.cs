using System.Runtime.CompilerServices;
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

    private float cellSizeCanvas;

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

        Canvas canvas = gridParent.GetComponentInParent<Canvas>();       

        float cellSizeCanvas = brickData.GetBrcikSize(canvas);
        float totalGridWidth = cellSizeCanvas * cols;
        float totalGridHeight = cellSizeCanvas * brickData.maxNumberRows;

        Vector2 startOffset = new Vector2 (0,0);

        Debug.Log($"StartOffset: {startOffset}, TotalGridWidth: {totalGridWidth}, TotalGridHeight: {totalGridHeight}, CellSize: {cellSizeCanvas}");

        for (int i = 0; i <= cols; i++)
        {
            float xPos = startOffset.x + i * cellSizeCanvas - (lineThickness / 2f);
            CreateLine(
                new Vector2(xPos, startOffset.y),
                new Vector2(xPos, startOffset.y + totalGridHeight),
                true
            );
        }

        // Lignes horizontales
        for (int j = 0; j <= brickData.maxNumberRows; j++)
        {
            float yPos = startOffset.y - j * cellSizeCanvas-(lineThickness / 2f); ; // inversion Y
            CreateLine(
                new Vector2(startOffset.x, yPos),
                new Vector2(startOffset.x + totalGridWidth, yPos),
                false
            );
        }
    }

    void CreateLine(Vector2 start, Vector2 end, bool isVertical)
    {
        string name = "Line ";
        name += (isVertical) ? "Vertical" : "Horizontal";

        GameObject line = new GameObject(name, typeof(Image));
        line.transform.SetParent(gridParent, false);

        RectTransform rt = line.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0, 1);

        float length = Vector2.Distance(start, end);
        rt.sizeDelta = new Vector2(lineThickness, length);

        rt.anchoredPosition = start;

        if (isVertical)
            rt.rotation = Quaternion.FromToRotation(Vector3.up, (end - start).normalized);
        else
            rt.rotation = Quaternion.AngleAxis(90, Vector3.forward);//*Quaternion.identity;;

        Image img = line.GetComponent<Image>();
        img.color = lineColor;
    }
}
