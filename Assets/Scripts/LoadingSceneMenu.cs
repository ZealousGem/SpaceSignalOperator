using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneMenu : BaseMainMenu
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float smoothSpeed = 5f;
    public Image LoadingBar;
    void Start()=> LoadScene();

    protected override void Awake()
    {
        if (!menu.activeSelf)
        {
            Menu(true);
        }
    }
    private void LoadScene()
    {
        if(LoadingManager.Instance == null) return;

        StartCoroutine(LoadSceneAsync(LoadingManager.Instance.SceneIndex));
    }

    private IEnumerator LoadSceneAsync(int sceneId)
    {

        if(LoadingBar == null) yield break;

        LoadingBar.fillAmount = 0;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneId);

        asyncOperation.allowSceneActivation = true;

        float progressValue = 0f;

        while (!asyncOperation.isDone)
        {
            progressValue = Mathf.Clamp01(asyncOperation.progress / 0.9f);

            LoadingBar.fillAmount = Mathf.MoveTowards(LoadingBar.fillAmount, progressValue, Time.deltaTime * smoothSpeed);

            yield return null;
        }

    }
}
