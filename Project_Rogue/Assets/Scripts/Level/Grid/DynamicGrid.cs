using UnityEngine;
using UnityEngine.UI;

public class DynamicGrid : MonoBehaviour
{
    public GameObject buttonPrefab; // Un bouton de base (UI Button avec Image)
    public RectTransform gridParent; // Le parent dans le Canvas
    [SerializeField] private BrickDatasSO brickData;

    private float cellSize;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        // Nettoie la grille existante
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        int cols = brickData.columns;
        int rows = brickData.rows;
  
        Canvas canvas = gridParent.GetComponentInParent<Canvas>();
        cellSize = brickData.GetBrcikSize(canvas);

        // Calcule l'offset pour centrer la grille
        float totalGridWidth = cellSize * cols;
        float totalGridHeight = cellSize * rows;

        Vector2 startOffset = Vector2.zero;

        // Crée les boutons
        for (int j = 0; j < brickData.maxNumberRows; j++)
        {
            for (int i = 0; i < cols; i++)
            {
                GameObject btn = Instantiate(buttonPrefab, gridParent);
                RectTransform rt = btn.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(cellSize, cellSize);
                rt.anchorMin = rt.anchorMax = new Vector2(0, 1);
                rt.pivot = new Vector2(0, 1);

                rt.anchoredPosition = new Vector2(
                    startOffset.x + i * cellSize,
                    startOffset.y - j * cellSize
                );
            }
        }
    }
}
