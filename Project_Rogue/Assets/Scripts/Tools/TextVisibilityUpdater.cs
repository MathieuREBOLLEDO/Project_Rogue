using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextVisibilityUpdater : MonoBehaviour
{
    private TextMeshProUGUI text;
    private void Awake() =>  text = GetComponent<TextMeshProUGUI>();
    private void SetTextVisibility(bool visibility) => text.enabled = visibility;
    public void CheckForVisibility (int value) => SetTextVisibility(value ==0 ? false : true);
}
