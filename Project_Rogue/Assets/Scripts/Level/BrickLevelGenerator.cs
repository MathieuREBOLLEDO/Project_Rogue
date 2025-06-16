using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

public class BrickLevelGenerator : MonoBehaviour
{

    [Header("Brick Settings")]
    public GameObject brickPrefab;        // Prefab de la brique carrée
    public int rows = 6;                  // Nombre de lignes de briques
    public int columns = 8;               // Nombre de colonnes de briques
    public float spacing = 0.1f;          // Espace entre les briques
        
    [Header("Preset (optionnel)")]
    public string presetFileName; // sans l’extension .json, ex: "Presets/niveau_facile"
    public bool usePreset = false;



    void Start()
    {
        GenerateBricks();
    }

    void SetBrickType(GameObject brick, int type)
    {
        // Exemple basique : couleur en fonction du type
        Color color = Color.white;
        switch (type)
        {
            case 1: color = Color.red; break;
            case 2: color = Color.green; break;
            case 3: color = Color.yellow; break;
                // ajoute d’autres types ici
        }

        SpriteRenderer sr = brick.GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.color = color;
    }


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
            // Si pas de preset, remplissage par défaut
            gridToUse = new int[rows, columns];
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < columns; c++)
                    gridToUse[r, c] = 1; // toutes les cases sont des briques simples
        }

        Vector2 screenMin = ScreenUtils.ScreenMin;
        Vector2 screenMax = ScreenUtils.ScreenMax;

        float brickSize = GetBrickSize();

        float startX = screenMin.x + brickSize / 2f;
        float startY = screenMax.y - brickSize / 2f;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                int type = gridToUse[row, col];
                if (type == 0)
                    continue; // vide

                float x = startX + col * (brickSize + spacing);
                float y = startY - row * (brickSize + spacing);
                Vector3 brickPos = new Vector3(x, y, 0);

                GameObject brick = Instantiate(brickPrefab, brickPos, Quaternion.identity, this.transform);
                brick.transform.localScale = new Vector3(brickSize, brickSize, 1f);

                // Si tu veux faire varier les couleurs ou types :
                SetBrickType(brick, type);
            }
        }
    }

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


    public void AddNewTopRow()
    {
        float brickSize = GetBrickSize();
        Vector2 screenMax = ScreenUtils.ScreenMax;
        float startY = screenMax.y - brickSize / 2f;

        // 1. Faire descendre toutes les briques d'une ligne
        foreach (Transform brick in transform)
        {
            brick.position += new Vector3(0, -(brickSize + spacing), 0);
        }

        // 2. Générer une nouvelle ligne de briques en haut
        float startX = ScreenUtils.ScreenMin.x + brickSize / 2f;

        for (int col = 0; col < columns; col++)
        {
            int type = Random.Range(1, 4); // aléatoire entre type 1, 2 ou 3
            float x = startX + col * (brickSize + spacing);
            float y = startY;

            Vector3 brickPos = new Vector3(x, y, 0);
            GameObject brick = Instantiate(brickPrefab, brickPos, Quaternion.identity, this.transform);
            brick.transform.localScale = new Vector3(brickSize, brickSize, 1f);
            SetBrickType(brick, type);
        }
    }
}
