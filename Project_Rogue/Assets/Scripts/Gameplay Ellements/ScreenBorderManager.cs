using UnityEngine;

public class ScreenBorderManager : MonoBehaviour
{
    [Header("Parents")]
    public Transform leftParent;
    public Transform rightParent;
    public Transform topParent;
    public Transform bottomParent;

    [Header("Debug Draw Collider (Runtime)")]
    public bool drawColliders = true;
    public Color drawColor = Color.red;
    public float lineWidth = 0.05f;

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

        // Update visuals after all modifications
        if (drawColliders)
        {
            UpdateColliderLines(leftParent);
            UpdateColliderLines(rightParent);
            UpdateColliderLines(topParent);
            UpdateColliderLines(bottomParent);
        }
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

            float width = col.size.x;
            col.size = new Vector2(width, segmentHeight);

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

            float height = col.size.y;
            col.size = new Vector2(segmentWidth, height);

            child.position = new Vector3(x, yEdge, 0);

            i++;
        }
    }

    #endregion

    #region Runtime Collider Drawing (LineRenderer)

    void UpdateColliderLines(Transform parent)
    {
        if (parent == null) return;

        foreach (Transform child in parent)
        {
            if (!child.TryGetComponent(out BoxCollider2D col)) continue;

            LineRenderer lr = child.GetComponent<LineRenderer>();
            if (lr == null)
                lr = child.gameObject.AddComponent<LineRenderer>();

            lr.useWorldSpace = true;
            lr.loop = true;
            lr.positionCount = 4;
            lr.startColor = drawColor;
            lr.endColor = drawColor;
            lr.startWidth = lineWidth;
            lr.endWidth = lineWidth;

            // Simple default material so it renders in builds
            if (lr.material == null)
                lr.material = new Material(Shader.Find("Sprites/Default"));

            // Collider corners in LOCAL space
            Vector2 size = col.size;
            Vector2 offset = col.offset;

            Vector3 p0 = new Vector3(offset.x - size.x / 2f, offset.y - size.y / 2f, 0);
            Vector3 p1 = new Vector3(offset.x - size.x / 2f, offset.y + size.y / 2f, 0);
            Vector3 p2 = new Vector3(offset.x + size.x / 2f, offset.y + size.y / 2f, 0);
            Vector3 p3 = new Vector3(offset.x + size.x / 2f, offset.y - size.y / 2f, 0);

            // Convert to WORLD space
            lr.SetPosition(0, child.TransformPoint(p0));
            lr.SetPosition(1, child.TransformPoint(p1));
            lr.SetPosition(2, child.TransformPoint(p2));
            lr.SetPosition(3, child.TransformPoint(p3));
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
