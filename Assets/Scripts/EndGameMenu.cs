using System;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameMenu : BaseMainMenu
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Button MenuButton; 
    public TMP_Text Title;
    public TMP_Text Reason;

    protected override void retrieveData(endGameUI data)
    {
        setButtonClickLinster(data.gameState);

        if (data.gameState == GameState.Success) EvokeMenu(data.Title, data.Reason, data.Amount);
        else if (data.gameState == GameState.Fail) EvokeMenu(data.Title, data.Reason);
        
    }

    private void setButtonClickLinster(GameState gameState)
    {
        if(MenuButton == null) return;

        TMP_Text ButtonText = MenuButton.gameObject.GetComponentInChildren<TMP_Text>();

        if(ButtonText == null) return;

        switch (gameState)
        {
            case GameState.Success: MenuButton.onClick.AddListener(NextLevel); ButtonText.text = "Next Delivery"; break;
            case GameState.Fail: MenuButton.onClick.AddListener(ResetLevel); ButtonText.text = "Restart Delivery"; break;
        }
    }

    private void NextLevel()
    {
         SceneManager.LoadScene(2);
    }

    private void ResetLevel()
    {
        SceneManager.LoadScene(1);
    }

    private void EvokeMenu(string title, string reason)
    {
        Menu(true);
        Title.text = title;
        Reason.text = reason;

    }

    protected void EvokeMenu(string title, string reason, float tim)
    {
        Menu(true);
        Title.text = title;
        TimeSpan timeSpan = TimeSpan.FromSeconds(tim);
        Reason.text = reason + "Your time was: " + timeSpan.ToString();
    }
}
