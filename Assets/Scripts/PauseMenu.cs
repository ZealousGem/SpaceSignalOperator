using UnityEngine;

public class PauseMenu : BaseMainMenu
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool isPaused = false;
    private bool isOver = true;

    private inGameOptionsMenu optionsMenu;

    protected override void Awake()
    {
        base.Awake();
        optionsMenu = GetComponent<inGameOptionsMenu>();
    }

    protected override void retrieveData(endGameUI data)
    {
        if (data.gameState == GameState.Ongoing || data.gameState == GameState.TutorialDone)
        {
            isOver = false;
        }

        else
        {
            isOver = true;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if(isOver) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandlePauseMenu();
        }
    }

    private void HandlePauseMenu()
    {
        if (isPaused)
        {
            UnPauseGame();
        }

        else  
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        Menu(true);
        SoundPlayer.PauseSound();

        Time.timeScale = 0f;
        isPaused = true;
    }

    public void UnPauseGame()
    {
        if (optionsMenu.menu.activeSelf && optionsMenu != null)
        {
          optionsMenu.Menu(false);  
        }

        else
        {
          Menu(false);
        }
        
        SoundPlayer.UnpauseSound();
        
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void OptionsMenu()
    {
        inGameOptionsMenu OptionsMenu = GetComponent<inGameOptionsMenu>();

        if(OptionsMenu == null) return;
        
        OptionsMenu.Menu(true);
        Menu(false);
    }

}
