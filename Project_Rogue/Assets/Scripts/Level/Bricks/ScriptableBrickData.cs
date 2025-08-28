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

        //float totalHSpacing = spacingWithBricks * (columns - 1);
        //float totalVSpacing = spacingWithBricks * (rows - 1);
        //
        //float brickWidth = (screenWidth - totalHSpacing) / columns;
        //float brickHeight = (screenHeight - totalVSpacing) / rows;
        //
        ////brickData.SetMaxRows(Mathf.FloorToInt(Screen.height / cellSize));
        //Debug.LogError($"ScreenWidth: {screenWidth}, ScreenHeight: {screenHeight}, Columns: {columns}, Rows: {rows}");

        return ComputeBrickSize(screenWidth, screenHeight);
        //return screenWidth / columns;
    }

    public float GetBrcikSize(Canvas canvas)
    {
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        float screenWidth = canvasRect.rect.width;
        float screenHeight = canvasRect.rect.height;

        

        return ComputeBrickSize(screenWidth, screenHeight);
    }

    private float ComputeBrickSize(float width, float height)
    {
        float totalHSpacing = spacingWithBricks * (columns - 1);
        float totalVSpacing = spacingWithBricks * (rows - 1);

        float brickWidth = (width - totalHSpacing) / columns;
        float brickHeight = (height - totalVSpacing) / rows;
        return Mathf.Min(brickWidth, brickHeight);
    }

    //public static float GetBrickSizeForCanvas(Canvas cs)
    //{
    //
    //    RectTransform canvasRect = cs.GetComponent<RectTransform>();
    //    float screenWidth = canvasRect.rect.width;
    //    float screenHeight = canvasRect.rect.height;
    //
    //    float totalHSpacing = spacingWithBricks * (columns - 1);
    //    float totalVSpacing = spacingWithBricks * (rows - 1);
    //
    //    float brickWidth = (screenWidth - totalHSpacing) / columns;
    //    float brickHeight = (screenHeight - totalVSpacing) / rows;
    //
    //    return Mathf.Min(brickWidth, brickHeight);
    //}



private int GetMaxRows()
    {
        Vector2 screenMin = ScreenUtils.ScreenMin;
        Vector2 screenMax = ScreenUtils.ScreenMax;
        float screenHeight = screenMax.y - screenMin.y;
        return Mathf.FloorToInt(screenHeight / cellSize);
    }
}
