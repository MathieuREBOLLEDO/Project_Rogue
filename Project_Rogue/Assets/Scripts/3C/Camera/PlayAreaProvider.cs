using UnityEngine;

public class PlayAreaProvider : MonoBehaviour
{
    public static PlayAreaProvider Instance { get; private set; }

    [Header("Borders parents (optionnels)")]
    public Transform leftParent;
    public Transform rightParent;
    public Transform topParent;
    public Transform bottomParent;

    public Rect PlayArea { get; private set; }
    public static bool IsReady { get; private set; }


    private bool initialized = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // On attend la fin de l'initialisation des autres scripts (borders, etc.)
        Invoke(nameof(CalculatePlayArea), 0.05f);
    }


    public void CalculatePlayArea()
    {
        // 1. Base = écran caméra
        Rect cameraRect = GetCameraWorldRect();

        float minX = cameraRect.xMin;
        float maxX = cameraRect.xMax;
        float minY = cameraRect.yMin;
        float maxY = cameraRect.yMax;

        // 2. Appliquer les borders si présents
        ApplyVerticalBorders(leftParent, ref minX, ref maxX, isLeft: true);
        ApplyVerticalBorders(rightParent, ref minX, ref maxX, isLeft: false);

        ApplyHorizontalBorders(bottomParent, ref minY, ref maxY, isBottom: true);
        ApplyHorizontalBorders(topParent, ref minY, ref maxY, isBottom: false);

        // 3. Sécurisation ABSOLUE
        float safeMinX = Mathf.Min(minX, maxX);
        float safeMaxX = Mathf.Max(minX, maxX);
        float safeMinY = Mathf.Min(minY, maxY);
        float safeMaxY = Mathf.Max(minY, maxY);

        PlayArea = Rect.MinMaxRect(safeMinX, safeMinY, safeMaxX, safeMaxY);

        initialized = true;

        Debug.Log("PlayAreaProvider -> Camera rect = " + cameraRect);
        Debug.Log("PlayAreaProvider -> PlayArea finale = " + PlayArea);

        IsReady = true;
    }

    public bool TryGetPlayArea(out Rect rect)
    {
        if (!IsReady || PlayArea.width <= 0 || PlayArea.height <= 0)
        {
            rect = default;
            return false;
        }

        rect = PlayArea;
        return true;
    }


    #region Borders reading

    void ApplyVerticalBorders(Transform parent, ref float minX, ref float maxX, bool isLeft)
    {
        if (parent == null || parent.childCount == 0)
            return;

        foreach (Transform child in parent)
        {
            if (!child.TryGetComponent(out BoxCollider2D col))
                continue;

            Bounds b = col.bounds;

            if (isLeft)
            {
                // Le border gauche pousse la limite min vers la droite
                minX = Mathf.Max(minX, b.max.x);
            }
            else
            {
                // Le border droit pousse la limite max vers la gauche
                maxX = Mathf.Min(maxX, b.min.x);
            }
        }
    }

    void ApplyHorizontalBorders(Transform parent, ref float minY, ref float maxY, bool isBottom)
    {
        if (parent == null || parent.childCount == 0)
            return;

        foreach (Transform child in parent)
        {
            if (!child.TryGetComponent(out BoxCollider2D col))
                continue;

            Bounds b = col.bounds;

            if (isBottom)
            {
                // Le border bas pousse la limite min vers le haut
                minY = Mathf.Max(minY, b.max.y);
            }
            else
            {
                // Le border haut pousse la limite max vers le bas
                maxY = Mathf.Min(maxY, b.min.y);
            }
        }
    }

    #endregion

    #region Camera fallback

    Rect GetCameraWorldRect()
    {
        Camera cam = Camera.main;

        float height = cam.orthographicSize * 2f;
        float width = height * cam.aspect;
        Vector3 c = cam.transform.position;

        return Rect.MinMaxRect(
            c.x - width / 2f,
            c.y - height / 2f,
            c.x + width / 2f,
            c.y + height / 2f
        );
    }

    #endregion

    #region Debug Gizmos

    private void OnDrawGizmos()
    {
        // On n'affiche les gizmos que pour la caméra principale
        if (Camera.current != Camera.main)
            return;

        if (PlayArea.width <= 0 || PlayArea.height <= 0)
            return;

        Gizmos.color = Color.yellow;

        Vector3 bl = new Vector3(PlayArea.xMin, PlayArea.yMin, 0);
        Vector3 br = new Vector3(PlayArea.xMax, PlayArea.yMin, 0);
        Vector3 tr = new Vector3(PlayArea.xMax, PlayArea.yMax, 0);
        Vector3 tl = new Vector3(PlayArea.xMin, PlayArea.yMax, 0);

        Gizmos.DrawLine(bl, br);
        Gizmos.DrawLine(br, tr);
        Gizmos.DrawLine(tr, tl);
        Gizmos.DrawLine(tl, bl);
    }


    #endregion
}
