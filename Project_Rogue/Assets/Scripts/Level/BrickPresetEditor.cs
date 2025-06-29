using System.IO;
using UnityEditor;
using UnityEngine;

public class BrickPresetEditor : EditorWindow
{
    private int rows = 6;
    private int columns = 8;
    private int[,] grid;
    private int selectedValue = 1;
    private Vector2 scrollPos;

    private const string saveFolder = "Assets/BrickPresets/";

    [MenuItem("Tools/Brick Preset Editor")]
    public static void ShowWindow()
    {
        GetWindow<BrickPresetEditor>("Brick Preset Editor");
    }

    private void OnEnable()
    {
        InitGrid();
    }

    void InitGrid()
    {
        grid = new int[rows, columns];
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Grille de brique", EditorStyles.boldLabel);

        rows = EditorGUILayout.IntField("Lignes", rows);
        columns = EditorGUILayout.IntField("Colonnes", columns);

        if (GUILayout.Button("Réinitialiser la grille"))
        {
            InitGrid();
        }

        selectedValue = EditorGUILayout.IntField("Valeur à dessiner (ex : 0, 1, 2...)", selectedValue);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int r = 0; r < rows; r++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int c = 0; c < columns; c++)
            {
                string label = grid[r, c].ToString();
                if (GUILayout.Button(label, GUILayout.Width(30), GUILayout.Height(30)))
                {
                    grid[r, c] = selectedValue;
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);

        if (GUILayout.Button("Sauvegarder en JSON"))
        {
            SavePreset();
        }

        if (GUILayout.Button("Charger un preset JSON"))
        {
            LoadPreset();
        }
    }

    void SavePreset()
    {
        if (!Directory.Exists(saveFolder))
            Directory.CreateDirectory(saveFolder);

        BrickPreset preset = new BrickPreset();
        preset.rows = rows;
        preset.columns = columns;

        for (int r = 0; r < rows; r++)
        {
            string line = "";
            for (int c = 0; c < columns; c++)
            {
                line += grid[r, c].ToString();
            }
            preset.grid.Add(line);
        }

        string json = JsonUtility.ToJson(preset, true);
        string path = EditorUtility.SaveFilePanel("Sauvegarder preset", saveFolder, "preset", "json");
        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllText(path, json);
            AssetDatabase.Refresh();
            Debug.Log("Preset sauvegardé : " + path);
        }
    }

    void LoadPreset()
    {
        string path = EditorUtility.OpenFilePanel("Charger preset", saveFolder, "json");
        if (!string.IsNullOrEmpty(path))
        {
            string json = File.ReadAllText(path);
            BrickPreset preset = JsonUtility.FromJson<BrickPreset>(json);

            rows = preset.rows;
            columns = preset.columns;
            grid = new int[rows, columns];

            for (int r = 0; r < rows; r++)
            {
                string line = preset.grid[r];
                for (int c = 0; c < Mathf.Min(line.Length, columns); c++)
                {
                    int.TryParse(line[c].ToString(), out grid[r, c]);
                }
            }

            Debug.Log("Preset chargé : " + path);
        }
    }
}

