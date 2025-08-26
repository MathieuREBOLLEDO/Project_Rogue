using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public int rows = 3; // nombre de lignes
    public int cols = 3; // nombre de colonnes
    public Vector2 cellSize = new Vector2(100, 100); // taille d'une case en pixels
    public Camera targetCamera;
    //public RectTransform gridParent; // Parent dans le Canvas pour les boutons

    void Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                Vector2 cellPos = new Vector2(x * cellSize.x, y * cellSize.y);

                // Instancie un bouton (préfab)
                GameObject button = new GameObject($"Button_{x}_{y}", typeof(RectTransform), typeof(UnityEngine.UI.Button), typeof(UnityEngine.UI.Image));
                button.transform.SetParent(this.transform, false);

                RectTransform rt = button.GetComponent<RectTransform>();
                rt.sizeDelta = cellSize;
                rt.anchoredPosition = cellPos;
            }
        }
    }
}
