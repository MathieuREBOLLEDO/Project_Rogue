using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DebugMenu : MonoBehaviour
{
//     public KeyCode toggleKey = KeyCode.F1;
//     private GameObject menuCanvas;
//     private bool isVisible = false;
// 
//     // Exemple de logs/debug info
//     private List<string> debugLogs = new List<string>();
//     private Text logText;
// 
//     void Awake()
//     {
//         CreateDebugMenuUI();
//         HideMenu();
//     }
// 
//     void Update()
//     {
//         if (Input.GetKeyDown(toggleKey))
//         {
//             isVisible = !isVisible;
//             menuCanvas.SetActive(isVisible);
//         }
//     }
// 
//     /// <summary>
//     /// Crée le menu UI dynamiquement.
//     /// </summary>
//     private void CreateDebugMenuUI()
//     {
//         // Canvas
//         menuCanvas = new GameObject("DebugMenuCanvas");
//         var canvas = menuCanvas.AddComponent<Canvas>();
//         canvas.renderMode = RenderMode.ScreenSpaceOverlay;
//         menuCanvas.AddComponent<CanvasScaler>();
//         menuCanvas.AddComponent<GraphicRaycaster>();
// 
//         // Panel
//         GameObject panel = new GameObject("Panel");
//         panel.transform.SetParent(menuCanvas.transform, false);
//         var image = panel.AddComponent<Image>();
//         image.color = new Color(0, 0, 0, 0.7f);
//         RectTransform panelRect = panel.GetComponent<RectTransform>();
//         panelRect.anchorMin = new Vector2(0.7f, 0f);
//         panelRect.anchorMax = new Vector2(1f, 1f);
//         panelRect.offsetMin = Vector2.zero;
//         panelRect.offsetMax = Vector2.zero;
// 
//         // Titre
//         GameObject title = new GameObject("Title");
//         title.transform.SetParent(panel.transform, false);
//         var titleText = title.AddComponent<Text>();
//         titleText.text = "DEBUG MENU";
//         titleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
//         titleText.color = Color.white;
//         titleText.fontSize = 24;
//         RectTransform titleRect = title.GetComponent<RectTransform>();
//         titleRect.anchorMin = new Vector2(0, 1);
//         titleRect.anchorMax = new Vector2(1, 1);
//         titleRect.pivot = new Vector2(0.5f, 1);
//         titleRect.offsetMin = new Vector2(10, -40);
//         titleRect.offsetMax = new Vector2(-10, -10);
// 
//         // Zone Logs
//         GameObject logArea = new GameObject("LogArea");
//         logArea.transform.SetParent(panel.transform, false);
//         logText = logArea.AddComponent<Text>();
//         logText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
//         logText.color = Color.green;
//         logText.alignment = TextAnchor.UpperLeft;
//         logText.fontSize = 14;
//         RectTransform logRect = logArea.GetComponent<RectTransform>();
//         logRect.anchorMin = new Vector2(0, 0);
//         logRect.anchorMax = new Vector2(1, 1);
//         logRect.offsetMin = new Vector2(10, 10);
//         logRect.offsetMax = new Vector2(-10, -50);
//     }
// 
//     /// <summary>
//     /// Cache le menu.
//     /// </summary>
//     public void HideMenu()
//     {
//         menuCanvas.SetActive(false);
//     }
// 
//     /// <summary>
//     /// Ajoute un log au menu debug.
//     /// </summary>
//     public void AddLog(string message)
//     {
//         debugLogs.Add(message);
//         if (debugLogs.Count > 30) // Limite des logs
//             debugLogs.RemoveAt(0);
// 
//         logText.text = string.Join("\n", debugLogs);
//     }
}
