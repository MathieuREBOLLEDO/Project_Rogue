using UnityEngine;
using UnityEngine.UI;

public class NewGridLines : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] BrickDatasSO brickData;
    [SerializeField] ScreenBorderManager borderManager;

    [Header("UI")]
    [SerializeField] RectTransform gridParent;
    [SerializeField] Color lineColor = Color.green;
    [SerializeField] float lineThickness = 2f;

    private void Start()
    {
        DrawGrid();
    }

    public void DrawGrid()
    {
        if (brickData == null || borderManager == null || gridParent == null)
        {
            Debug.LogError("GridLines : références manquantes");
            return;
        }

        // Nettoyage des anciennes lignes
        for (int i = gridParent.childCount - 1; i >= 0; i--)
        {
            if (gridParent.GetChild(i).name.StartsWith("Line"))
                Destroy(gridParent.GetChild(i).gameObject);
        }

        Canvas canvas = gridParent.GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("GridLines : Canvas introuvable");
            return;
        }

        // --- ZONE JOUABLE À PARTIR DES COLLIDERS ---
        Rect playAreaWorld = GetPlayAreaWorld();
        Rect playAreaCanvas = WorldToCanvasRect(playAreaWorld, canvas);

        int cols = brickData.columns;
        int rows = brickData.maxNumberRows;

        float totalGridWidth = playAreaCanvas.width;
        float totalGridHeight = playAreaCanvas.height;

        float cellWidth = totalGridWidth / cols;
        float cellHeight = totalGridHeight / rows;

        // Origine en haut à gauche (UI)
        Vector2 startOffset = new Vector2(
            playAreaCanvas.xMin,
            playAreaCanvas.yMax
        );

        // --- LIGNES VERTICALES ---
        for (int i = 0; i <= cols; i++)
        {
            float xPos = startOffset.x + i * cellWidth - (lineThickness * 0.5f);

            CreateLine(
                new Vector2(xPos, startOffset.y),
                new Vector2(xPos, startOffset.y - totalGridHeight),
                true
            );
        }

        // --- LIGNES HORIZONTALES ---
        for (int j = 0; j <= rows; j++)
        {
            float yPos = startOffset.y - j * cellHeight - (lineThickness * 0.5f);

            CreateLine(
                new Vector2(startOffset.x, yPos),
                new Vector2(startOffset.x + totalGridWidth, yPos),
                false
            );
        }
    }

    #region Play Area Calculation

    private Rect GetPlayAreaWorld()
    {
        float minX = float.NegativeInfinity;
        float maxX = float.PositiveInfinity;
        float minY = float.NegativeInfinity;
        float maxY = float.PositiveInfinity;

        //foreach (var b in borderManager.leftBorders)
        //    minX = Mathf.Max(minX, b.GetComponent<BoxCollider2D>().bounds.max.x);
        //
        //foreach (var b in borderManager.rightBorders)
        //    maxX = Mathf.Min(maxX, b.GetComponent<BoxCollider2D>().bounds.min.x);
        //
        //foreach (var b in borderManager.bottomBorders)
        //    minY = Mathf.Max(minY, b.GetComponent<BoxCollider2D>().bounds.max.y);
        //
        //foreach (var b in borderManager.topBorders)
        //    maxY = Mathf.Min(maxY, b.GetComponent<BoxCollider2D>().bounds.min.y);

        return Rect.MinMaxRect(minX, minY, maxX, maxY);
    }

    private Rect WorldToCanvasRect(Rect worldRect, Canvas canvas)
    {
        RectTransform canvasRT = canvas.transform as RectTransform;

        Vector2 minScreen = Camera.main.WorldToScreenPoint(worldRect.min);
        Vector2 maxScreen = Camera.main.WorldToScreenPoint(worldRect.max);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRT, minScreen, canvas.worldCamera, out Vector2 minLocal);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRT, maxScreen, canvas.worldCamera, out Vector2 maxLocal);

        return Rect.MinMaxRect(minLocal.x, minLocal.y, maxLocal.x, maxLocal.y);
    }

    #endregion

    #region Line Creation

    private void CreateLine(Vector2 start, Vector2 end, bool isVertical)
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

    #endregion
}
