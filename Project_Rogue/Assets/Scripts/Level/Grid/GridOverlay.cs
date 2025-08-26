using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridOverlay : MonoBehaviour
{
    public int gridWidth = 10;
    public int gridHeight = 10;
    public float cellSize = 50f; // en pixels UI
    public Color lineColor = Color.green;
    public GameObject brickPrefab;
    //Spublic Camera gridCamera; // ta caméra secondaire

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        // Adapter la grille à la zone de la caméra
       // Rect vp = Camera.main.pixelRect;
       // rectTransform.anchorMin = Vector2.zero;
       // rectTransform.anchorMax = Vector2.zero;
       // rectTransform.pivot = Vector2.zero;
       // rectTransform.anchoredPosition = new Vector2(vp.x, vp.y);
       // rectTransform.sizeDelta = new Vector2(vp.width, vp.height);

        DrawGrid();
    }

    void DrawGrid()
    {
        for (int x = 0; x <= gridWidth; x++)
            CreateLine(new Vector2(x * cellSize, 0), new Vector2(x * cellSize, gridHeight * cellSize));

        for (int y = 0; y <= gridHeight; y++)
            CreateLine(new Vector2(0, y * cellSize), new Vector2(gridWidth * cellSize, y * cellSize));
    }

    void CreateLine(Vector2 start, Vector2 end)
    {
        GameObject line = new GameObject("Line", typeof(Image));
        line.transform.SetParent(transform, false);
        Image img = line.GetComponent<Image>();
        img.color = lineColor;

        RectTransform rt = img.rectTransform;
        Vector2 dir = (end - start);
        float length = dir.magnitude;
        rt.sizeDelta = new Vector2(length, 2f); // épaisseur 2px
        rt.pivot = new Vector2(0, 0.5f);
        rt.anchoredPosition = start;
        rt.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
    }

    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    // Vérifier que le clic est dans la zone de la caméra
        //    Vector3 mousePos = Input.mousePosition;
        //    Ray ray = Camera.main.ScreenPointToRay(mousePos);
        //
        //    if (Physics.Raycast(ray, out RaycastHit hit))
        //    {
        //        Vector3 worldPos = hit.point;
        //
        //        // Convertir les coordonnées monde en "cellules"
        //        int x = Mathf.FloorToInt(worldPos.x);
        //        int y = Mathf.FloorToInt(worldPos.y);
        //
        //        Vector3 spawnPos = new Vector3(x + 0.5f, y + 0.5f, 0);
        //        Instantiate(brickPrefab, spawnPos, Quaternion.identity);
        //    }
        //}
    }
}

