using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="BrickDatas",menuName ="Brcik/Brcik Datas")]
public class ScriptableBrickData : ScriptableObject
{
    public int columns = 8;
    public int rows = 4;

    public float cellSize;

    public int maxNumberRows;

    public int spacingWithScreen = 0;
    public int spacingWithBricks = 0;

    void OnEnable()
    {
        cellSize = GetBrickSize();
        Debug.Log("cell size " + cellSize); 
        maxNumberRows  = GetMaxRows();
    }

    private float GetBrickSize()
    {
        Vector2 screenMin = ScreenUtils.ScreenMin;
        Vector2 screenMax = ScreenUtils.ScreenMax;
        float screenWidth = screenMax.x - screenMin.x;
        float screenHeight = screenMax.y - screenMin.y;

        float totalHSpacing = spacingWithBricks * (columns - 1);
        float totalVSpacing = spacingWithBricks * (rows - 1);

        float brickWidth = (screenWidth - totalHSpacing) / columns;
        float brickHeight = (screenHeight - totalVSpacing) / rows;

        //brickData.SetMaxRows(Mathf.FloorToInt(Screen.height / cellSize));

        return Mathf.Min(brickWidth, brickHeight);
        //return screenWidth / columns;
    }

    private int GetMaxRows()
    {
        Vector2 screenMin = ScreenUtils.ScreenMin;
        Vector2 screenMax = ScreenUtils.ScreenMax;
        float screenHeight = screenMax.y - screenMin.y;
        return Mathf.FloorToInt(screenHeight / cellSize);
    }
}
