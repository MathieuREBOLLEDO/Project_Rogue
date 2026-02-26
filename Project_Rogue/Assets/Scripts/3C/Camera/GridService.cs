using UnityEngine;

public class GridService : MonoBehaviour
{

    public static GridService Instance { get; private set; }

    [SerializeField] GridConfigSO gridConfig;

    public int Columns => gridConfig.columns;
    public int Rows => gridConfig.rows;

    private void Awake()
    {
        Instance = this;
    }

    // Taille d'une cellule en world space
    public Vector2 CellSize
    {
        get
        {
            if (!PlayAreaProvider.IsReady)
            {
                Debug.LogWarning("GridService: PlayArea pas prête");
                return Vector2.zero;
            }

            Rect area = PlayAreaProvider.Instance.PlayArea;

            return new Vector2(
                area.width / gridConfig.columns,
                area.height / gridConfig.rows
            );
        }
    }

    // Centre d'une cellule (col, row) — row 0 en bas
    public Vector3 GetCellCenter(int col, int row)
    {
        Rect area = PlayAreaProvider.Instance.PlayArea;
        Vector2 cell = CellSize;

        float x = area.xMin + cell.x * (col + 0.5f);
        float y = area.yMin + cell.y * (row + 0.5f);

        return new Vector3(x, y, 0);
    }

    // Ligne Y donnée (pour générer une rangée de bricks)
    public float GetRowY(int row)
    {
        Rect area = PlayAreaProvider.Instance.PlayArea;
        Vector2 cell = CellSize;

        return area.yMin + cell.y * (row + 0.5f);
    }
}
