using System.Collections;
using UnityEngine;

public class BrickLevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private int startLines = 3;

    private void Start()
    {
        StartCoroutine(WaitForPlayAreaAndInit());
    }

    private void OnEnable()
    {
        GridEvents.OnRequestNewBrickLine += AddNewTopLine;
        GameEventManager.Instance.Subscribe( GameEventType.MachineTurnStart,_ => AddNewTopLine());
    }

    private void OnDisable()
    {
        GridEvents.OnRequestNewBrickLine -= AddNewTopLine;
        GameEventManager.Instance.Unsubscribe(GameEventType.MachineTurnStart, _=> AddNewTopLine());
    }

    private IEnumerator WaitForPlayAreaAndInit()
    {
        while (PlayAreaProvider.Instance == null ||
               PlayAreaProvider.Instance.PlayArea.width <= 0)
        {
            yield return null;
        }

        for (int i = 0; i < startLines; i++)
            AddNewTopLine();
    }

    /// <summary>
    /// Descend toutes les briques d'une ligne
    /// puis ajoute une nouvelle ligne en haut
    /// </summary>
    public void AddNewTopLine()
    {
        MoveAllBricksDown();
        CreateLine(GridService.Instance.Rows - 1);
        GameManager.Instance.NotifyMachineTurnEnd();
    }

    private void MoveAllBricksDown()
    {
        Vector2 cellSize = GridService.Instance.CellSize;

        foreach (Transform brick in transform)
        {
            brick.position += Vector3.down * cellSize.y;
        }
    }

    private void CreateLine(int row)
    {
        int cols = GridService.Instance.Columns;
        Vector2 cellSize = GridService.Instance.CellSize;

        float margin = 0.9f;
        Vector3 brickScale = new Vector3(
            cellSize.x * margin,
            cellSize.y * margin,
            1f
        );

        for (int col = 0; col < cols; col++)
        {
            Vector3 position = GridService.Instance.GetCellCenter(col, row);
            GameObject brick = Instantiate(brickPrefab, position, Quaternion.identity, transform);
            brick.transform.localScale = brickScale;
        }
    }
}
