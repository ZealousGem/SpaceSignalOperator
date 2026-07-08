using System;
using System.Collections.Generic;
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

        if (data.gameState == GameState.Success) EvokeMenu(data.Title, data.Reason, data.Amount, data.StarRating, data.scoreList);
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
       //  SceneManager.LoadScene(2);
    }

    private void ResetLevel()
    {
        SoundPlayer.StopAllInGameSounds();
        
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        LoadingNextScene.LoadScene(currentSceneIndex);
    }

    private void EvokeMenu(string title, string reason)
    {
        Menu(true);

        if (Stars.activeSelf || StarBorder.activeSelf) HandleStarSystem(false); 

        SoundPlayer.PlaySound("LevelFailed");

        Title.text = title;
        Reason.text = reason;

    }

    protected void EvokeMenu(string title, string reason, float tim, int StarAmount, List<float> ScoreList)
    {
        Menu(true);
        Title.text = title;

        SoundPlayer.PlaySound("LevelComplete");

        StarRating(StarAmount, ScoreList);

        TimeSpan timeSpan = TimeSpan.FromSeconds(tim);
        
        Reason.text = reason + "Your time was " + timeSpan.ToString(@"mm\:ss\:fff");
    }

    private void StarRating(int amount, List<float> ScoreAmount)
    {
        if(ScoreAmount == null || StarBorder == null || ScoreAmount == null) return;

        if (!Stars.activeSelf || !StarBorder.activeSelf) HandleStarSystem(true); 
        
        int totalStars = Stars.transform.childCount;
        int borderStarAmount = StarBorder.transform.childCount;

        amount = Mathf.Clamp(amount, 0, Mathf.Min(totalStars, borderStarAmount));
       // Debug.Log(amount);

        for (int i = 0; i < totalStars; i++)
        {
            Transform childStar = Stars.transform.GetChild(i);
            childStar.gameObject.SetActive(i < amount);

            if (i > 0 && i <= ScoreAmount.Count)
            {
                Transform star = StarBorder.transform.GetChild(i);
                DisplayStarTimer(ScoreAmount[i-1], star.gameObject);
            }        
            
        }  

    }

    private void DisplayStarTimer(float timer, GameObject Star)
    {
      
      if(Star == null) throw new Exception("What Star Lol");
      
      TMP_Text timeUi = Star.GetComponentInChildren<TMP_Text>();

      if(timeUi == null) throw new Exception("Text does not Exist");
      
      TimeSpan timeSpan = TimeSpan.FromSeconds(timer);
      timeUi.text = timeSpan.ToString(@"mm\:ss");

    }
}
