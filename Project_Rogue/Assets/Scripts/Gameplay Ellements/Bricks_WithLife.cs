using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bricks_WithLife : Bricks
{
    [SerializeField] TMPro.TextMeshPro displayText;

    private int numberOfHits = 10;

    //private void Start()
    //{
    //    numberOfHits = 10;
    //}

    protected override void _NotifyTriggered()
    {
        if (numberOfHits > 0)
        {
            numberOfHits--;
            if (displayText != null)
                displayText.text = numberOfHits.ToString();
        }
        else
        {
            _NotifyDestroy();
        }
    }

}
