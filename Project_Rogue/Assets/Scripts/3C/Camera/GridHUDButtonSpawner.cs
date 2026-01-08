using UnityEngine;
using UnityEngine.UI;

public class GridHUDButtonSpawner : MonoBehaviour
{
    public Button buttonPrefab;
    public Canvas canvas;

    private void Start()
    {
        SpawnButtons();
    }

    void SpawnButtons()
    {
        for (int r = 0; r < GridService.Instance.Rows; r++)
        {
            for (int c = 0; c < GridService.Instance.Columns; c++)
            {
                Vector3 worldPos = GridService.Instance.GetCellCenter(c, r);
                Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

                Button btn = Instantiate(buttonPrefab, canvas.transform);
                btn.transform.position = screenPos;

                int col = c;
                int row = r;
                btn.onClick.AddListener(() =>
                {
                    Debug.Log($"Click sur cellule [{col},{row}]");
                });
            }
        }
    }
}
