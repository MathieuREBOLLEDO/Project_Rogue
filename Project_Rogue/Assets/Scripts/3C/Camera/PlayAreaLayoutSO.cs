using UnityEngine;
using System;
using System.Collections.Generic;

public enum ZoneType
{
    Bricks,
    Danger,
    Bumpers,
    Player
}


[CreateAssetMenu(menuName = "PlayArea/Layout")]
public class PlayAreaLayoutSO : ScriptableObject
{
    public GridConfigSO globalGrid;

    public List<ZoneDef> zones = new List<ZoneDef>();

    public bool TryGetZone(ZoneType type, out ZoneDef def)
    {
        def = zones.Find(z => z.type == type);
        return def != null;
    }
}

[Serializable]
public class ZoneDef
{
    public ZoneType type;
    public int rows;
    public Color debugColor;
}
