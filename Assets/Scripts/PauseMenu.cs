using UnityEngine;

public class PauseMenu : BaseMainMenu
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool isPaused = false;
    private bool isOver = true;

    protected override void retrieveData(endGameUI data)
    {
        if (data.gameState == GameState.Ongoing)
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
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void UnPauseGame()
    {
        Menu(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

}
