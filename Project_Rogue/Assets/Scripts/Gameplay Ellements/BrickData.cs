using UnityEngine;
using UnityEngine.UIElements;

public enum BrickType
{
    Basic,
    Strong,
    Unbreakable
}


public class BrickData
{

    public int Column;
    public int Row;
    public bool IsActive;
    public BrickType Type;

    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;
    public Transform Parent;

    public BrickData(int col, int row)
    {
        Column = col;
        Row = row;
        IsActive = true;


    }
    public BrickData( Vector3 pos, Quaternion rot , Vector3 scale , Transform prt)
    {
        Position = pos;
        Rotation = rot;
        Scale = scale;
        Parent = prt;

    }

    public void SetBrickType(BrickType type)
    {
        Type = type;
    }
}