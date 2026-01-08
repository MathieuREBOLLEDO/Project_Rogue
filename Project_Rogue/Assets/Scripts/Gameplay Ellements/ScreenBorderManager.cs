using UnityEngine;

public class ScreenBorderManager : MonoBehaviour
{
    [Header("Parents")]
    public Transform leftParent;
    public Transform rightParent;
    public Transform topParent;
    public Transform bottomParent;

    private void Awake()
    {
        PositionAndResizeBorders();
    }

    public void PositionAndResizeBorders()
    {
        Rect camRect = GetCameraWorldRect();

        // LEFT
        PositionVerticalBordersKeepWidth(leftParent, camRect.xMin);

        // RIGHT
        PositionVerticalBordersKeepWidth(rightParent, camRect.xMax);

        // TOP
        PositionHorizontalBordersKeepHeight(topParent, camRect.yMax);

        // BOTTOM
        PositionHorizontalBordersKeepHeight(bottomParent, camRect.yMin);
    }

    #region Vertical borders (LEFT / RIGHT)

    void PositionVerticalBordersKeepWidth(Transform parent, float xEdge)
    {
        if (parent == null || parent.childCount == 0) return;

        Rect camRect = GetCameraWorldRect();
        float totalHeight = camRect.height;

        int count = parent.childCount;
        float segmentHeight = totalHeight / count;
        float startY = camRect.yMin + segmentHeight / 2f;

        int i = 0;
        foreach (Transform child in parent)
        {
            if (!child.TryGetComponent(out BoxCollider2D col)) continue;

            float y = startY + i * segmentHeight;

            // on garde la largeur existante
            float width = col.size.x;

            // on adapte uniquement la hauteur
            col.size = new Vector2(width, segmentHeight);

            // position : collé au bord X, réparti en Y
            child.position = new Vector3(xEdge, y, 0);

            i++;
        }
    }

    #endregion

    #region Horizontal borders (TOP / BOTTOM)

    void PositionHorizontalBordersKeepHeight(Transform parent, float yEdge)
    {
        if (parent == null || parent.childCount == 0) return;

        Rect camRect = GetCameraWorldRect();
        float totalWidth = camRect.width;

        int count = parent.childCount;
        float segmentWidth = totalWidth / count;
        float startX = camRect.xMin + segmentWidth / 2f;

        int i = 0;
        foreach (Transform child in parent)
        {
            if (!child.TryGetComponent(out BoxCollider2D col)) continue;

            float x = startX + i * segmentWidth;

            // on garde la hauteur existante
            float height = col.size.y;

            // on adapte uniquement la largeur
            col.size = new Vector2(segmentWidth, height);

            // position : collé au bord Y, réparti en X
            child.position = new Vector3(x, yEdge, 0);

            i++;
        }
    }

    #endregion

    #region Camera rect

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
}
