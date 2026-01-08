using System.Collections.Generic;
using UnityEngine;

public static class ScreenUtils
{
    // -------------------------
    // PLAY AREA (SOURCE DE VÉRITÉ)
    // -------------------------
    private static Rect? playAreaOverride = null;

    /// <summary>
    /// Zone de jeu courante en WORLD SPACE.
    /// Si aucune PlayArea n’est définie, on retourne les bounds caméra.
    /// </summary>
    public static Rect PlayArea
    {
        get
        {
            if (playAreaOverride.HasValue)
                return playAreaOverride.Value;

            // Fallback = bounds caméra
            Camera cam = Camera.main;
            float height = cam.orthographicSize * 2f;
            float width = height * cam.aspect;
            Vector3 center = cam.transform.position;

            return Rect.MinMaxRect(
                center.x - width / 2f,
                center.y - height / 2f,
                center.x + width / 2f,
                center.y + height / 2f
            );
        }
    }

    public static void SetPlayArea(Rect worldRect)
    {
        playAreaOverride = worldRect;
    }

    public static void ClearPlayArea()
    {
        playAreaOverride = null;
    }

    // -------------------------
    // COMPATIBILITÉ ANCIEN CODE
    // -------------------------
    public static Vector2 ScreenMin
    {
        get
        {
            Debug.Log(playAreaOverride.HasValue
                ? "ScreenUtils : PlayArea utilisée"
                : "ScreenUtils : Camera utilisée");

            return new Vector2(PlayArea.xMin, PlayArea.yMin);
        }
    }

    public static Vector2 ScreenMax
    {
        get
        {
            return new Vector2(PlayArea.xMax, PlayArea.yMax);
        }
    }

    // -------------------------
    // DEBUG
    // -------------------------
    public static void DebugDrawPlayArea(float duration = 5f)
    {
        //Rect r = PlayArea;
        //
        //Debug.DrawLine(new Vector3(r.xMin, r.yMin), new Vector3(r.xMax, r.yMin), Color.green, duration);
        //Debug.DrawLine(new Vector3(r.xMax, r.yMin), new Vector3(r.xMax, r.yMax), Color.green, duration);
        //Debug.DrawLine(new Vector3(r.xMax, r.yMax), new Vector3(r.xMin, r.yMax), Color.green, duration);
        //Debug.DrawLine(new Vector3(r.xMin, r.yMax), new Vector3(r.xMin, r.yMin), Color.green, duration);
    }

    public static bool IsVisible(Vector2 pos)
    {
        Vector2 min = ScreenMin;
        Vector2 max = ScreenMax;

        return pos.x >= min.x && pos.x <= max.x &&
               pos.y >= min.y && pos.y <= max.y;
    }

    public static List<ScreenBounds> GetTouchedEdges(Vector2 pos, float radius)
    {
        Vector2 min = ScreenMin;
        Vector2 max = ScreenMax;
        List<ScreenBounds> edges = new();

        if (pos.x - radius < min.x) edges.Add(ScreenBounds.Left);
        if (pos.x + radius > max.x) edges.Add(ScreenBounds.Right);
        if (pos.y - radius < min.y) edges.Add(ScreenBounds.Bottom);
        if (pos.y + radius > max.y) edges.Add(ScreenBounds.Top);

        return edges;
    }

    public static bool TouchesBottom(Vector2 pos, float radius)
    {
        return pos.y - radius <= ScreenMin.y;
    }

    public static bool IsBelowScreen(Vector2 pos, float radius)
    {
        return pos.y + radius < ScreenMin.y;
    }

    public static Vector2 ReflectDirection(Vector2 direction, List<ScreenBounds> edges)
    {
        foreach (var edge in edges)
        {
            switch (edge)
            {
                case ScreenBounds.Left:
                    direction = Vector2.Reflect(direction, Vector2.right);
                    break;
                case ScreenBounds.Right:
                    direction = Vector2.Reflect(direction, Vector2.left);
                    break;
                case ScreenBounds.Top:
                    direction = Vector2.Reflect(direction, Vector2.down);
                    break;
                case ScreenBounds.Bottom:
                    direction = Vector2.Reflect(direction, Vector2.up);
                    break;
            }
        }

        direction.Normalize();
        return direction;
    }

    public static Vector3 ClampToScreen(Vector3 pos, float radius)
    {
        Vector2 min = ScreenMin;
        Vector2 max = ScreenMax;

        float x = Mathf.Clamp(pos.x, min.x + radius, max.x - radius);
        float y = Mathf.Clamp(pos.y, min.y + radius, max.y - radius);
        return new Vector3(x, y, pos.z);
    }

    public static Vector3 ConvertScreenToWorld(Vector2 pos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(pos);
        worldPos.z = 0;
        return worldPos;
    }

    public static Vector2 SimulateBounce(Vector2 position, Vector2 direction, float radius, float step)
    {
        Vector2 newPos = position + direction * step;
        var edges = GetTouchedEdges(newPos, radius);

        if (edges.Count > 0)
        {
            direction = ReflectDirection(direction, edges);
            newPos = position + direction * step;
        }

        return newPos;
    }
}
