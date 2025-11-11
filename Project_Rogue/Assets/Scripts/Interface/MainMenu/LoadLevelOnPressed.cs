using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LoadLevelOnPressed : MonoBehaviour
{
    public int sceneIndexToLoad = -1;
    public int sceneIndexToUnload = -1; 
    public LoadMethod loadMethod; 
    public void LoadLevel()
    {
        LoadSceneController.Instance.SetData(sceneIndexToLoad, loadMethod);
        LoadSceneController.Instance.LoadScene();
        LoadSceneController.Instance.SetData(sceneIndexToUnload);
        LoadSceneController.Instance.UnloadScene();
    }

    //LoadLevelOnPressed
}