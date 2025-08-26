using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditorInternal.VersionControl.ListControl;

public class BrickLevelGenerator : MonoBehaviour
{
    [Header("Config")]
    public bool useSeed = false;
    public int seed = 0;
    public int rows = 4;
    public int columns = 8;

    [SerializeField] ScriptableBrickData brickData;

    public List<WeightedLine> presetLinePool;
    public GameObject brickPrefab;

    public float spacing = 0.1f;

    private IRandomProvider randomProvider;
    private ILineGenerator lineGenerator;

    GameState lastState;

    private void OnEnable() => EventBus.OnGameStateChanged += HandleGameStateChanged;
    private void OnDisable() => EventBus.OnGameStateChanged -= HandleGameStateChanged;

    void Awake()
    {
        randomProvider = new SeededRandomProvider(useSeed ? seed : System.DateTime.Now.Millisecond);
        lineGenerator = new WeightedLineGenerator(presetLinePool, randomProvider);
    }

    void Start()
    {
        GenerateInitialBricks();
    }

    void GenerateInitialBricks()
    {
        for (int i = 0; i < rows; i++)
            AddProceduralLineAt();
    }

    private void HandleGameStateChanged(GameState state)
    {

        Debug.LogWarning(state.ToString ());
        if (state == lastState) return; // Ignore les états répétés

        lastState = state;

        if (state == GameState.MachineTurn)
            AddProceduralLineAt();
    }

    void AddProceduralLineAt()
    {
        float brickSize = brickData.cellSize;
        float verticalOffset = brickSize + spacing;

        // Déplace toutes les briques existantes vers le bas
        foreach (Transform child in transform)
            child.position -= new Vector3(0, verticalOffset, 0);

        // Calcul de la position Y pour la nouvelle ligne
        float startY = ScreenUtils.ScreenMax.y - brickSize / 2f;
        float startX = ScreenUtils.ScreenMin.x + brickSize / 2f;

        string line = lineGenerator.GenerateLine();

        for (int col = 0; col < columns; col++)
        {
            int type = (col < line.Length) ? int.Parse(line[col].ToString()) : 0;
            if (type == 0) continue;

            float x = startX + col * (brickSize + spacing);
            Vector3 pos = new Vector3(x, startY, 0);

            GameObject brickGO = Instantiate(brickPrefab, pos, Quaternion.identity, transform);
            brickGO.transform.localScale = new Vector3(brickSize, brickSize, 1f);

            if (brickGO.TryGetComponent(out Bricks brck))
                brck.Initialize(type);
        }
    }

    float GetBrickSize()
    {
        Vector2 screenMin = ScreenUtils.ScreenMin;
        Vector2 screenMax = ScreenUtils.ScreenMax;
        float screenWidth = screenMax.x - screenMin.x;
        float screenHeight = screenMax.y - screenMin.y;

        float totalHSpacing = spacing * (columns - 1);
        float totalVSpacing = spacing * (rows - 1);

        float brickWidth = (screenWidth - totalHSpacing) / columns;
        float brickHeight = (screenHeight - totalVSpacing) / rows;

        return Mathf.Min(brickWidth, brickHeight);
    }

}
