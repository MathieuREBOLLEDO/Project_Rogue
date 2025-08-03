using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TextUpdaterForCanvas : MonoBehaviour
{
    TMPro.TextMeshProUGUI displayText;

    private void Start()
    {
        displayText = GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void UpdateText(int number)
    {
        displayText.text = number.ToString();
    }
}
