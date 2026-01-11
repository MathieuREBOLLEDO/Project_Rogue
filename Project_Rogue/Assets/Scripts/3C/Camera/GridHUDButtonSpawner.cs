using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GridHUDButtonSpawner : MonoBehaviour
{
    public GridConfigSO gridConfig;
    public Button buttonPrefab;
    public Canvas canvas;

    [Header("Spawn")]
    public GameObject objectToSpawn;   // l’objet à créer dans le monde

    private void Start()
    {
        StartCoroutine(WaitAndSpawn());
    }

    IEnumerator WaitAndSpawn()
    {
        while (!PlayAreaProvider.IsReady)
            yield return null;

        SpawnButtons();
    }

    void SpawnButtons()
    {
        Vector2 cellSize = GridService.Instance.CellSize;

        for (int r = 0; r < gridConfig.rows; r++)
        {
            for (int c = 0; c < gridConfig.columns; c++)
            {
                // position monde de la cellule
                Vector3 worldPos = GridService.Instance.GetCellCenter(c, r);

                // création du bouton
                Button btn = Instantiate(buttonPrefab, canvas.transform);

                // position dans le monde
                btn.transform.position = worldPos;

                // donner la taille de la cellule
                RectTransform rt = btn.GetComponent<RectTransform>();

                Vector3 canvasScale = canvas.transform.lossyScale;

                rt.sizeDelta = new Vector2(
                    cellSize.x / canvasScale.x,
                    cellSize.y / canvasScale.y
                );

                int col = c;
                int row = r;

                // clic = spawn objet
                btn.onClick.AddListener(() =>
                {
                    Destroy( btn );
                    SpawnObjectAt(col, row);
                });
            }
        }
    }

    void SpawnObjectAt(int col, int row)
    {
        if (objectToSpawn == null)
        {
            Debug.LogWarning("Aucun prefab à spawner");
            return;
        }


        Vector3 pos = GridService.Instance.GetCellCenter(col, row);
        GameObject obj = Instantiate(objectToSpawn, pos, Quaternion.identity);

        // on adapte la taille à la grille
        ResizeObjectToCell(obj);


        Debug.Log($"Objet spawn à la case [{col},{row}]");
    }

    void ResizeObjectToCell(GameObject obj)
    {
        Vector2 cellSize = GridService.Instance.CellSize;

        // marge pour éviter de toucher les murs
        float margin = 0.9f;

        Vector3 targetSize = new Vector3(
            cellSize.x * margin,
            cellSize.y * margin,
            1f
        );

        // cas 2D avec SpriteRenderer
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Vector2 spriteSize = sr.bounds.size;

            float scaleX = targetSize.x / spriteSize.x;
            float scaleY = targetSize.y / spriteSize.y;

            obj.transform.localScale = new Vector3(scaleX, scaleY, 1f);
            return;
        }

        // fallback simple
        obj.transform.localScale = targetSize;
    }

}
