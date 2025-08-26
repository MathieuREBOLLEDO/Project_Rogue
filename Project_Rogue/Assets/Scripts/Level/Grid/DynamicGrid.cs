using UnityEngine;
using UnityEngine.UI;

public class DynamicGrid : MonoBehaviour
{
    //public int rows = 6;
    //public int cols = 4;
    public GameObject buttonPrefab; // Un bouton de base (UI Button avec Image)
    public RectTransform gridParent; // Le parent dans le Canvas
    [SerializeField] private ScriptableBrickData brickData;

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
        //Vector2 screenMin = ScreenUtils.ScreenMin;
        //Vector2 screenMax = ScreenUtils.ScreenMax;
        //float screenWidth = screenMax.x - screenMin.x;
        //float screenHeight = screenMax.y - screenMin.y;
        // Récupère taille écran
        // float screenWidth = Screen.width;
        //float screenHeight = Screen.height;

        // Calcule la taille d'une case carrée
        //float cellWidth = screenWidth / cols;
        //float cellHeight = screenHeight / rows;
        //cellSize = Mathf.Min(cellWidth, cellHeight);

        cellSize = brickData.cellSize;
        //cellSize = BrickUtils.GetBrickSize();

        // Calcule l'offset pour centrer la grille
        float totalGridWidth = cellSize * cols;
        float totalGridHeight = cellSize * rows;
        //Vector2 startOffset = new Vector2(
        //    (screenWidth - totalGridWidth) / 2f,
        //    (screenHeight - totalGridHeight) / 2f
        //);

        Vector2 startOffset = Vector2.zero;

        // Crée les boutons
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                GameObject btn = Instantiate(buttonPrefab, gridParent);
                RectTransform rt = btn.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(cellSize, cellSize);
                rt.anchorMin = rt.anchorMax = new Vector2(0, 0); // bas-gauche
                rt.pivot = new Vector2(0, 0); // pivot en bas-gauche

                rt.anchoredPosition = new Vector2(
                    startOffset.x + x * cellSize,
                    startOffset.y + y * cellSize
                );
            }
        }
    }
}
