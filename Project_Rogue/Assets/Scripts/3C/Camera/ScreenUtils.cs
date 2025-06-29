using System.Collections.Generic;
using UnityEngine;

public enum ScreenBounds
{
    None,
    Left,
    Right,
    Bottom,
    Top,
}

public static class ScreenUtils
{

    public static Vector2 ScreenMin
    {
        get
        {
            Camera cam = Camera.main;
            float height = cam.orthographicSize * 2f;
            float width = height * cam.aspect;
            Vector3 center = cam.transform.position;
            return new Vector2(center.x - width / 2f, center.y - height / 2f);
        }
    }

    public static Vector2 ScreenMax
    {
        get
        {
            Camera cam = Camera.main;
            float height = cam.orthographicSize * 2f;
            float width = height * cam.aspect;
            Vector3 center = cam.transform.position;
            return new Vector2(center.x + width / 2f, center.y + height / 2f);
        }
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
            if (edge == ScreenBounds.Left || edge == ScreenBounds.Right)
                direction.x *= -1;
            else if (edge == ScreenBounds.Top)//|| edge == ScreenBounds.Bottom)
                direction.y *= -1;
        }
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
}


