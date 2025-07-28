using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextUpdater : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshPro displayText;
    public void UpdateText(int number)
    {
        displayText.text = number.ToString();
    }
}
