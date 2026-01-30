using UnityEngine;
using System.Collections.Generic;

public struct ZoneData
{
    public Rect rect;
    public Color color;
}

public class PlayAreaLayoutBuilder : MonoBehaviour
{
    public PlayAreaLayoutSO layout;

    public float CellWidth { get; private set; }
    public float CellHeight { get; private set; }

    public Dictionary<ZoneType, ZoneData> Zones { get; private set; }

    public bool Build()
    {
        Zones = new Dictionary<ZoneType, ZoneData>();

        if (PlayAreaProvider.Instance == null) return false;
        if (!PlayAreaProvider.Instance.TryGetPlayArea(out Rect area)) return false;
        if (layout == null || layout.globalGrid == null) return false;

        CellWidth = area.width / layout.globalGrid.columns;
        CellHeight = area.height / layout.globalGrid.rows;

        float currentTop = area.yMax;

        foreach (var def in layout.zones)
        {
            float h = def.rows * CellHeight;

            Rect r = Rect.MinMaxRect(
                area.xMin,
                currentTop - h,
                area.xMax,
                currentTop
            );

            Zones.Add(def.type, new ZoneData
            {
                rect = r,
                color = def.debugColor
            });

            currentTop -= h;
        }

        return true;
    }
}
