using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextUpdaterForObject : MonoBehaviour
{
    TMPro.TextMeshPro displayText;

    private void Awake()
    {
        displayText = GetComponent<TMPro.TextMeshPro>();
    }

    public void UpdateText(int number)
    {
        displayText.text = number.ToString();
    }
}
