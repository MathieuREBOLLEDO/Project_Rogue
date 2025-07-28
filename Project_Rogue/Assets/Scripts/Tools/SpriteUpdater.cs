using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteUpdater : MonoBehaviour
{
    SpriteRenderer sr;
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public void UpdateColor(int number)
    {
        ///TO DO -> Update Color with ratio => need a number max of life point (limited to the display ratio)
        /*
        Color color = number switch
        {
            1 => Color.green,
            2 => Color.yellow,
            3 => Color.blue,
            4 => Color.magenta,
            5 => Color.red,
            _ => Color.white
        };
        */
        Color color = Color.red;
        //color.a = 0.2f;
        sr.color = color;
    }

}
