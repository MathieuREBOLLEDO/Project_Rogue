using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GridHUDButtonSpawner : MonoBehaviour
{
    public static GridHUDButtonSpawner Instance { get; private set; }

    [Header("Layout")]
    private PlayAreaLayoutBuilder layoutBuilder;
    public ZoneType targetZone;

    private int rows;
    private int columns;

    [Header("UI")]
    public Button buttonPrefab;
    public Canvas canvas;

    [Header("Spawn")]
    public GameObject objectToSpawn;

    private Rect zoneRect;
    private Vector2 cellSize;

    private void Start()
    {      
        if (Instance == null) 
            Instance = this;
            
        //StartCoroutine(WaitAndSpawn());
    }

   // IEnumerator WaitAndSpawn()
   // {
   //     while (!PlayAreaProvider.IsReady)
   //         yield return null;
   //
   //     if (!layoutBuilder.Build())
   //     {
   //         Debug.LogError("PlayAreaLayoutBuilder.Build() a échoué");
   //         yield break;
   //     }
   //
   //     if (!layoutBuilder.Zones.TryGetValue(targetZone, out ZoneData zone))
   //     {
   //         Debug.LogError($"Zone {targetZone} introuvable");
   //         yield break;
   //     }
   //
   //     if (layoutBuilder == null) yield break;
   //     if (!layoutBuilder.Build()) yield break; 
   //     if (!layoutBuilder.Zones.TryGetValue(targetZone, out ZoneData data)) yield break;
   //
   //     columns = layoutBuilder.layout.globalGrid.columns;
   //     rows = layoutBuilder.layout.GetZoneData(targetZone).rows;
   //
   //     zoneRect = zone.rect;
   //
   //     cellSize = new Vector2(
   //         zoneRect.width / columns,
   //         zoneRect.height / rows
   //     );
   //
   //     SpawnButtons();
   // }

    public void initDataBBeforeSpawnButtons(PlayAreaLayoutBuilder builder)
    {      
        layoutBuilder = builder;

        if (!layoutBuilder.Build())
        {
            Debug.LogError("PlayAreaLayoutBuilder.Build() a échoué");
            return;
        }
      
       if (!layoutBuilder.Zones.TryGetValue(targetZone, out ZoneData zone))
       {
           Debug.LogError($"Zone {targetZone} introuvable");
           return;
       }

        if (layoutBuilder == null) return;
        if (!layoutBuilder.Build()) return;
        if (!layoutBuilder.Zones.TryGetValue(targetZone, out ZoneData data)) return;

        columns = layoutBuilder.layout.globalGrid.columns;
        rows = layoutBuilder.layout.GetZoneData(targetZone).rows;

        zoneRect = zone.rect;

        cellSize = new Vector2(
            zoneRect.width / columns,
            zoneRect.height / rows
        );

        SpawnButtons();
    }

    void SpawnButtons()
    {
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                Vector3 worldPos = GetCellCenter(c, r);

                Button btn = Instantiate(buttonPrefab, canvas.transform);
                btn.transform.position = worldPos;

                RectTransform rt = btn.GetComponent<RectTransform>();
                Vector3 canvasScale = canvas.transform.lossyScale;

                rt.sizeDelta = new Vector2(
                    cellSize.x / canvasScale.x,
                    cellSize.y / canvasScale.y
                );

                int col = c;
                int row = r;

                btn.onClick.AddListener(() =>
                {
                    Destroy(btn.gameObject);
                    SpawnObjectAt(col, row);
                });
            }
        }
    }

    Vector3 GetCellCenter(int col, int row)
    {
        float x = zoneRect.xMin + (col + 0.5f) * cellSize.x;
        float y = zoneRect.yMax - (row + 0.5f) * cellSize.y;
        return new Vector3(x, y, 0f);
    }

    void SpawnObjectAt(int col, int row)
    {
        if (objectToSpawn == null)
        {
            Debug.LogWarning("Aucun prefab à spawner");
            return;
        }

        Vector3 pos = GetCellCenter(col, row);
        GameObject obj = Instantiate(objectToSpawn, pos, Quaternion.identity);

        ResizeObjectToCell(obj);

        Debug.Log($"Objet spawn à [{col},{row}] dans zone {targetZone}");
    }

    void ResizeObjectToCell(GameObject obj)
    {
        float margin = 0.9f;

        Vector3 targetSize = new Vector3(
            cellSize.x * margin,
            cellSize.y * margin,
            1f
        );

        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Vector2 spriteSize = sr.bounds.size;

            float scaleX = targetSize.x / spriteSize.x;
            float scaleY = targetSize.y / spriteSize.y;

            obj.transform.localScale = new Vector3(scaleX, scaleY, 1f);
            return;
        }

        obj.transform.localScale = targetSize;
    }
}
