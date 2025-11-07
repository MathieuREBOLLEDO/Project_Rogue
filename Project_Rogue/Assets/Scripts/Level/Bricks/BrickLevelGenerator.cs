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

    [SerializeField] BrickDatasSO brickData;

    public List<WeightedLine> presetLinePool;
    public GameObject brickPrefab;

    public float spacing = 0.1f;

    private IRandomProvider randomProvider;
    private ILineGenerator lineGenerator;

    GameEventType lastEvent;

    private void Start()
    {
        GenerateInitialBricks();
        GameEventManager.Instance.Subscribe(GameEventType.MachineTurnStart, HandleGameStateChanged);
    }
    private void OnDisable()
    {
        GameEventManager.Instance.Unsubscribe(GameEventType.MachineTurnStart, HandleGameStateChanged);
    }

    void Awake()
    {
        randomProvider = new SeededRandomProvider(useSeed ? seed : System.DateTime.Now.Millisecond);
        lineGenerator = new WeightedLineGenerator(presetLinePool, randomProvider);
    }

    void GenerateInitialBricks()
    {
        for (int i = 0; i < rows; i++)
            AddProceduralLineAt();
    }

    private void HandleGameStateChanged(GameEvent gameEvent)
    {
        Debug.LogWarning(gameEvent.eventType.ToString());
        if (gameEvent.eventType == lastEvent) return; // Ignore les états répétés

        lastEvent = gameEvent.eventType;

        if (gameEvent.eventType == GameEventType.MachineTurnStart)
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
}
