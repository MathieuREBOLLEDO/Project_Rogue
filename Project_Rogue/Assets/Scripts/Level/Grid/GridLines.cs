using UnityEngine;
using UnityEngine.UI;

public class GridLines : MonoBehaviour
{
    [SerializeField] BrickDatasSO brickData;

    public RectTransform gridParent;
    public Color lineColor = Color.green;
    public float lineThickness = 2f;

    private void Start()
    {
        DrawGrid();
    }

    public void DrawGrid()
    {
        int cols = brickData.columns;
        int rows = brickData.maxNumberRows;

        // Nettoyage
        foreach (Transform child in gridParent)
        {
            if (child.name.StartsWith("Line"))
                Destroy(child.gameObject);
        }

        Canvas canvas = gridParent.GetComponentInParent<Canvas>();

        // --- taille d’une cellule en UI ---
        float cellSizeCanvas = brickData.GetBrcikSize(canvas);

        // --- ZONE JOUABLE ---
        Rect playArea = ScreenUtils.PlayArea;

        // Largeur/hauteur max disponibles en WORLD
        float playWidth = playArea.width;
        float playHeight = playArea.height;

        // Conversion WORLD -> CANVAS (approx simple)
        float totalGridWidth = cellSizeCanvas * cols;
        float totalGridHeight = cellSizeCanvas * rows;

        // Point de départ = coin haut gauche de la PlayArea
        Vector2 startOffset = new Vector2(
            playArea.xMin,
            playArea.yMax - totalGridHeight
        );

        Debug.Log($"Grid StartOffset: {startOffset}");

        // --- LIGNES VERTICALES ---
        for (int i = 0; i <= cols; i++)
        {
            float xPos = startOffset.x + i * cellSizeCanvas - (lineThickness / 2f);

            CreateLine(
                new Vector2(xPos, startOffset.y),
                new Vector2(xPos, startOffset.y + totalGridHeight),
                true
            );
        }

        // --- LIGNES HORIZONTALES ---
        for (int j = 0; j <= rows; j++)
        {
            float yPos = startOffset.y - j * cellSizeCanvas - (lineThickness / 2f);

            CreateLine(
                new Vector2(startOffset.x, yPos),
                new Vector2(startOffset.x + totalGridWidth, yPos),
                false
            );
        }
    }

    void CreateLine(Vector2 start, Vector2 end, bool isVertical)
    {
        string name = "Line_" + (isVertical ? "Vertical" : "Horizontal");

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
            rt.rotation = Quaternion.AngleAxis(90f, Vector3.forward);

        Image img = line.GetComponent<Image>();
        img.color = lineColor;
    }
}
