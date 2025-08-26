using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public static class BrickUtils 
{
    public static ScriptableBrickData brickData;

    public static void SetBrickData(ScriptableBrickData data) => brickData = data;

    public static float GetBrickSize()
    {
        Vector2 screenMin = ScreenUtils.ScreenMin;
        Vector2 screenMax = ScreenUtils.ScreenMax;
        float screenWidth = screenMax.x - screenMin.x;
        float screenHeight = screenMax.y - screenMin.y;

        float totalHSpacing = brickData.spacingWithBricks * (brickData.columns - 1);
        float totalVSpacing = brickData.spacingWithBricks * (brickData.rows - 1);

        float brickWidth = (screenWidth - totalHSpacing) / brickData.columns;
        float brickHeight = (screenHeight - totalVSpacing) / brickData.rows;

        //brickData.SetMaxRows(Mathf.FloorToInt(Screen.height / cellSize));

        //return Mathf.Min(brickWidth, brickHeight);
        return screenWidth /brickData.columns;
    }
}
