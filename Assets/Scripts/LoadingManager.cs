using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : Singleton<LoadingManager>
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    [HideInInspector]
    public int SceneIndex = 0; 
    private readonly int LoadingSceneIndex = 1;

    public void LoadScene(int _SceneIndex)
    {
        SceneIndex = _SceneIndex;
        SceneManager.LoadScene(LoadingSceneIndex);
    }
}

public static class LoadingNextScene
{
    public static void LoadScene(int SceneNo)
    {
        if(LoadingManager.Instance == null) return;
        LoadingManager.Instance.LoadScene(SceneNo);
    }
}
