using UnityEngine;
using System.Collections.Generic;

public class ScreenBorderManager : MonoBehaviour
{
    [Header("Parents")]
    public Transform topParent;
    public Transform bottomParent;
    public Transform leftParent;
    public Transform rightParent;

    [Header("Borders")]
    public List<BorderElement> topBorders = new();
    public List<BorderElement> bottomBorders = new();
    public List<BorderElement> leftBorders = new();
    public List<BorderElement> rightBorders = new();

    [Header("Settings")]
    public float borderThickness = 0.5f;

    private void Awake()
    {
        InitBorders();
        PositionAndResizeBorders();
    }

    #region Init

    private void InitBorders()
    {
        ClearLists();

        CollectBorders(topParent, topBorders);
        CollectBorders(bottomParent, bottomBorders);
        CollectBorders(leftParent, leftBorders);
        CollectBorders(rightParent, rightBorders);
    }

    private void CollectBorders(Transform parent, List<BorderElement> list)
    {
        if (parent == null) return;

        foreach (Transform child in parent)
        {
            BorderElement border = child.GetComponent<BorderElement>();
            if (border != null)
                list.Add(border);
        }
    }

    private void ClearLists()
    {
        topBorders.Clear();
        bottomBorders.Clear();
        leftBorders.Clear();
        rightBorders.Clear();
    }

    #endregion

    #region Placement & Resize

    private void PositionAndResizeBorders()
    {
        Vector2 min = ScreenUtils.ScreenMin;
        Vector2 max = ScreenUtils.ScreenMax;

        ResizeHorizontal(topBorders, max.y);
        ResizeHorizontal(bottomBorders, min.y);

        ResizeVertical(leftBorders, min.x);
        ResizeVertical(rightBorders, max.x);
    }

    private void ResizeHorizontal(List<BorderElement> borders, float yPos)
    {
        if (borders.Count == 0) return;

        float width = (ScreenUtils.ScreenMax.x - ScreenUtils.ScreenMin.x) / borders.Count;

        for (int i = 0; i < borders.Count; i++)
        {
            BorderElement b = borders[i];

            float x = ScreenUtils.ScreenMin.x + width * (i + 0.5f);
            b.transform.position = new Vector3(x, yPos, 0);

            SetSize(b, width, borderThickness);
        }
    }

    private void ResizeVertical(List<BorderElement> borders, float xPos)
    {
        if (borders.Count == 0) return;

        float height = (ScreenUtils.ScreenMax.y - ScreenUtils.ScreenMin.y) / borders.Count;

        for (int i = 0; i < borders.Count; i++)
        {
            BorderElement b = borders[i];

            float y = ScreenUtils.ScreenMin.y + height * (i + 0.5f);
            b.transform.position = new Vector3(xPos, y, 0);

            SetSize(b, borderThickness, height);
        }
    }

private void SetSize(BorderElement border, float width, float height)
{
    // Collider
    BoxCollider2D col = border.GetComponent<BoxCollider2D>();
    if (col != null)
        col.size = new Vector2(width, height);

    // Visuel scale
    SpriteRenderer sr = border.GetComponent<SpriteRenderer>();
    if (sr != null)
    {
        Vector2 spriteSize = sr.sprite.bounds.size;

        border.transform.localScale = new Vector3(
            width / spriteSize.x,
            height / spriteSize.y,
            1f
        );
    }
}



#endregion

#region Public API

    public void SetBordersActive(ScreenBounds side, bool value)
    {
        foreach (var b in GetBordersBySide(side))
            b.SetActive(value);
    }

    private List<BorderElement> GetBordersBySide(ScreenBounds side)
    {
        return side switch
        {
            ScreenBounds.Top => topBorders,
            ScreenBounds.Bottom => bottomBorders,
            ScreenBounds.Left => leftBorders,
            ScreenBounds.Right => rightBorders,
            _ => null
        };
    }

    #endregion
}
