using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseMainMenu : MonoBehaviour
{
    public GameObject menu;
    // Start is called once before the first execution of Update after the MonoBehaviour is create

    protected virtual void OnEnable()
    {
        EventBus.Subscribe<endGameUI>(retrieveData);
    }

    protected virtual void OnDisable()
    {
       EventBus.Unsubscribe<endGameUI>(retrieveData); 
    }

    protected virtual void retrieveData(endGameUI data){}

    protected virtual void Awake()
    {
        Menu(false);
    }
    
    public virtual void Menu(bool state)
    {
        if(menu == null) return;

        menu.SetActive(state);
    }

    public void ReturnToMainMenu()
    {
        SoundPlayer.StopAllInGameSounds();
        SceneManager.LoadScene(1);
    } 
    
}
