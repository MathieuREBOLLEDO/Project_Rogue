using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableGridButton : MonoBehaviour
{
    public PlayAreaLayoutBuilder layoutBuilder;
    public void EnableButton()
    {
        GridHUDButtonSpawner.Instance.initDataBBeforeSpawnButtons(layoutBuilder);
    }
}
