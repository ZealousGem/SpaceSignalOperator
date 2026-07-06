using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameMenu : BaseMainMenu
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Button MenuButton; 
    public TMP_Text Title;
    public TMP_Text Reason;
    public GameObject StarBorder;
    public GameObject Stars;

    protected override void Awake()
    {
        base.Awake();
        HandleStarSystem(false);
    }

    private void HandleStarSystem(bool state)
    {
        if(StarBorder == null || Stars == null) throw new InvalidOperationException("Stars is not instantiated.");

        if(!StarBorder.activeSelf || !Stars.activeSelf) return;

        StarBorder.SetActive(state);
        Stars.SetActive(state);
    }

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
         SoundPlayer.StopAllInGameSounds();
         SceneManager.LoadScene(2);
    }

    private void ResetLevel()
    {
        SoundPlayer.StopAllInGameSounds();
        
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex);
    }

    private void EvokeMenu(string title, string reason)
    {
        Menu(true);
        SoundPlayer.PlaySound("LevelFailed");

        Title.text = title;
        Reason.text = reason;

    }

    protected void EvokeMenu(string title, string reason, float tim)
    {
        Menu(true);
        Title.text = title;

        SoundPlayer.PlaySound("LevelComplete");

        

        TimeSpan timeSpan = TimeSpan.FromSeconds(tim);
        
        Reason.text = reason + "Your time was " + timeSpan.ToString(@"mm\:ss\:fff");
    }

    private void StarRating(int amount)
    {
        HandleStarSystem(true);

        GameObject[] ColouredStars = Stars.GetComponentsInChildren<GameObject>();

        if(amount > ColouredStars.Length) amount = ColouredStars.Length;

        for (int i = 0; i < amount; i++)
        {
            ColouredStars[i].SetActive(true);
        }  

    }
}
