using UnityEngine;
using UnityEngine.SceneManagement;

public enum LoadMethod
{
    Single,     // Remplace la scène actuelle
    Additive    // Ajoute la scène sans fermer l’actuelle
}

public class LoadSceneController : MonoBehaviour
{
    public static LoadSceneController Instance;

    [Header("Scene à gérer")]
    public string sceneName;
    public int sceneIndex = -1;

    [Header("Méthode de chargement")]
    public LoadMethod loadMethod = LoadMethod.Additive;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetData(int index)
    {
        sceneIndex = index;
    }

    public void SetData(int index, LoadMethod method)
    {
        sceneIndex = index;
        loadMethod = method;
    }

    public void SetData(string name)
    {
        sceneName = name;
    }


    public void SetData(string name, LoadMethod method)
    {
        sceneName = name;
        loadMethod = method;
    }


    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName,
                loadMethod == LoadMethod.Additive ? LoadSceneMode.Additive : LoadSceneMode.Single);
            return;
        }

        if (sceneIndex >= 0)
        {
            SceneManager.LoadScene(sceneIndex,
                loadMethod == LoadMethod.Additive ? LoadSceneMode.Additive : LoadSceneMode.Single);
            return;
        }

        Debug.LogWarning("[LoadSceneController] Aucune scène définie.");
    }

    public void UnloadScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            if (SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                SceneManager.UnloadSceneAsync(sceneName);
            }
            return;
        }

        if (sceneIndex >= 0)
        {
            Scene scene = SceneManager.GetSceneByBuildIndex(sceneIndex);
            if (scene.isLoaded)
            {
                SceneManager.UnloadSceneAsync(sceneIndex);
            }
            return;
        }

        Debug.LogWarning("[LoadSceneController] Aucune scène définie.");
    }
}
