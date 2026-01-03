using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class OnPressedEvent : MonoBehaviour
{
    public UnityEvent OnPressed;

    public void CallOnPressed()
    {
        OnPressed?.Invoke();
    }
}
