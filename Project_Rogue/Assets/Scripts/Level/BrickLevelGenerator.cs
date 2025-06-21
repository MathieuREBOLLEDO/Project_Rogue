using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BrickLevelGenerator : MonoBehaviour
{
    #region Variables
    [Header("Procedural Generation")]
    public bool useSeed = false;
    public int seed = 0;
    public int linesToGenerate = 1;

    [Header("Weighted Preset Line Pool")]
    public List<WeightedLine> presetLinePool;

    [Header("Brick Settings")]
    public GameObject brickPrefab;
    public int rows = 6;
    public int columns = 8;
    public float spacing = 0.1f;

    [Header("Preset (optionnel)")]
    public string presetFileName; // sans l’extension .json
    public bool usePreset = false;

    #endregion
    void Start()
    {
        GenerateBricks();
    }

    // Génère les briques en fonction d’un preset ou d’une grille par défaut
    void GenerateBricks()
    {
        int[,] gridToUse;

        if (usePreset && !string.IsNullOrEmpty(presetFileName))
        {
            TextAsset jsonFile = Resources.Load<TextAsset>(presetFileName);
            if (jsonFile != null)
            {
                BrickPreset preset = JsonUtility.FromJson<BrickPreset>(jsonFile.text);
                rows = preset.rows;
                columns = preset.columns;
                gridToUse = new int[rows, columns];

                for (int r = 0; r < preset.grid.Count; r++)
                {
                    string line = preset.grid[r];
                    for (int c = 0; c < Mathf.Min(line.Length, columns); c++)
                    {
                        int.TryParse(line[c].ToString(), out gridToUse[r, c]);
                    }
                }
            }
            else
            {
                Debug.LogWarning("Fichier preset non trouvé : " + presetFileName);
                return;
            }
        }
        else
        {
            gridToUse = new int[rows, columns];
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < columns; c++)
                    gridToUse[r, c] = 1;
        }

        float brickSize = GetBrickSize();
        Vector2 screenMin = ScreenUtils.ScreenMin;
        Vector2 screenMax = ScreenUtils.ScreenMax;
        float startX = screenMin.x + brickSize / 2f;
        float startY = screenMax.y - brickSize / 2f;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                int type = gridToUse[row, col];
                if (type == 0) continue;

                float x = startX + col * (brickSize + spacing);
                float y = startY - row * (brickSize + spacing);
                Vector3 brickPos = new Vector3(x, y, 0);

                GameObject brick = Instantiate(brickPrefab, brickPos, Quaternion.identity, transform);
                brick.transform.localScale = new Vector3(brickSize, brickSize, 1f);
                SetBrickType(brick, type);
            }
        }
    }

    #region Methodes
    // Détermine la taille des briques en fonction de l’écran
    float GetBrickSize()
    {
        Vector2 screenMin = ScreenUtils.ScreenMin;
        Vector2 screenMax = ScreenUtils.ScreenMax;
        float screenWidth = screenMax.x - screenMin.x;
        float screenHeight = screenMax.y - screenMin.y;

        float totalHorizontalSpacing = spacing * (columns - 1);
        float totalVerticalSpacing = spacing * (rows - 1);

        float brickWidth = (screenWidth - totalHorizontalSpacing) / columns;
        float brickHeight = (screenHeight - totalVerticalSpacing) / rows;

        return Mathf.Min(brickWidth, brickHeight);
    }

    // Applique un type de brique en modifiant sa couleur
    void SetBrickType(GameObject brick, int type)
    {
        Color color = Color.white;
        switch (type)
        {
            case 1: color = Color.red; break;
            case 2: color = Color.green; break;
            case 3: color = Color.yellow; break;
        }

        SpriteRenderer sr = brick.GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = color;
    }

    // Ajoute plusieurs lignes procédurales en haut du niveau
    public void AddProceduralLines()
    {
        float brickSize = GetBrickSize();
        float startY = ScreenUtils.ScreenMax.y - brickSize / 2f;

        foreach (Transform brick in transform)
        {
            brick.position += new Vector3(0, -(brickSize + spacing) * linesToGenerate, 0);
        }

        System.Random rng = useSeed ? new System.Random(seed) : new System.Random();

        for (int line = 0; line < linesToGenerate; line++)
        {
            string newLine = GetWeightedRandomLine(rng);
            float y = startY - line * (brickSize + spacing);
            float startX = ScreenUtils.ScreenMin.x + brickSize / 2f;

            for (int col = 0; col < columns; col++)
            {
                int type = 0;
                if (col < newLine.Length)
                    int.TryParse(newLine[col].ToString(), out type);

                if (type == 0) continue;

                float x = startX + col * (brickSize + spacing);
                Vector3 pos = new Vector3(x, y, 0);

                GameObject brick = Instantiate(brickPrefab, pos, Quaternion.identity, transform);
                brick.transform.localScale = new Vector3(brickSize, brickSize, 1f);
                SetBrickType(brick, type);
            }
        }
    }

    // Ajoute une nouvelle ligne aléatoire en haut
    public void AddNewTopRow()
    {
        float brickSize = GetBrickSize();
        float startY = ScreenUtils.ScreenMax.y - brickSize / 2f;

        foreach (Transform brick in transform)
        {
            brick.position += new Vector3(0, -(brickSize + spacing), 0);
        }

        float startX = ScreenUtils.ScreenMin.x + brickSize / 2f;

        for (int col = 0; col < columns; col++)
        {
            int type = Random.Range(1, 4);
            float x = startX + col * (brickSize + spacing);
            Vector3 brickPos = new Vector3(x, startY, 0);

            GameObject brick = Instantiate(brickPrefab, brickPos, Quaternion.identity, transform);
            brick.transform.localScale = new Vector3(brickSize, brickSize, 1f);
            SetBrickType(brick, type);
        }
    }

    // Sélectionne une ligne aléatoire pondérée depuis la pool
    string GetWeightedRandomLine(System.Random rng)
    {
        int totalWeight = 0;
        foreach (var entry in presetLinePool)
            totalWeight += entry.weight;

        int randomWeight = rng.Next(0, totalWeight);
        int current = 0;

        foreach (var entry in presetLinePool)
        {
            current += entry.weight;
            if (randomWeight < current)
                return entry.line;
        }

        return new string('1', columns);
    }

    // Exporte la disposition actuelle des briques vers un fichier JSON
    public void ExportCurrentLevelToJson(string fileName = "generated_level.json")
    {
        BrickPreset preset = new BrickPreset
        {
            rows = rows,
            columns = columns,
            grid = new List<string>()
        };

        float brickSize = GetBrickSize();
        Vector2 screenMin = ScreenUtils.ScreenMin;
        Vector2 screenMax = ScreenUtils.ScreenMax;
        float startX = screenMin.x + brickSize / 2f;
        float startY = screenMax.y - brickSize / 2f;

        Dictionary<Vector2Int, int> brickMap = new Dictionary<Vector2Int, int>();

        foreach (Transform brick in transform)
        {
            Vector3 pos = brick.position;
            int col = Mathf.RoundToInt((pos.x - startX) / (brickSize + spacing));
            int row = Mathf.RoundToInt((startY - pos.y) / (brickSize + spacing));

            int type = 1;
            SpriteRenderer sr = brick.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Color color = sr.color;
                if (color == Color.red) type = 1;
                else if (color == Color.green) type = 2;
                else if (color == Color.yellow) type = 3;
            }

            brickMap[new Vector2Int(row, col)] = type;
        }

        for (int r = 0; r < rows; r++)
        {
            string line = "";
            for (int c = 0; c < columns; c++)
            {
                Vector2Int key = new Vector2Int(r, c);
                int type = brickMap.ContainsKey(key) ? brickMap[key] : 0;
                line += type.ToString();
            }
            preset.grid.Add(line);
        }

        string json = JsonUtility.ToJson(preset, true);
        string path = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(path, json);
        Debug.Log("Preset exported to: " + path);
    }
    #endregion
}