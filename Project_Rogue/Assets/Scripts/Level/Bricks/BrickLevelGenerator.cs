using System.Collections;
using UnityEngine;

public class BrickLevelGenerator : MonoBehaviour
{
    [SerializeField] GameObject brickPrefab;

    private int currentTopRow;

    private void Start()
    {
        StartCoroutine(WaitAndGenerate());
    }

    IEnumerator WaitAndGenerate()
    {
        // on attend que la PlayArea soit valide
        while (PlayAreaProvider.Instance == null ||
               PlayAreaProvider.Instance.PlayArea.width <= 0)
        {
            yield return null;
        }

        GenerateInitialBricks();
    }

    private void OnEnable()
    {
        GridEvents.OnRequestNewBrickLine += AddProceduralLine;
    }
    private void OnDisable()
    {
        GridEvents.OnRequestNewBrickLine -= AddProceduralLine;
    }

    void GenerateInitialBricks()
    {
        currentTopRow = GridService.Instance.Rows - 1;

        // Génère 3 lignes au départ (exemple)
        for (int i = 0; i < 3; i++)
            AddProceduralLine();
    }

    public void AddProceduralLine()
    {
        int cols = GridService.Instance.Columns;

        Vector2 cellSize = GridService.Instance.CellSize;

        // petite marge pour éviter de toucher les murs
        float margin = 0.9f;

        Vector3 brickScale = new Vector3(
            cellSize.x * margin,
            cellSize.y * margin,
            1f
        );

        for (int col = 0; col < cols; col++)
        {
            Vector3 pos = GridService.Instance.GetCellCenter(col, currentTopRow);

            GameObject brick = Instantiate(brickPrefab, pos, Quaternion.identity, transform);

            // on force la taille logique de la brique
            brick.transform.localScale = brickScale;
        }

        currentTopRow--;
    }

}
